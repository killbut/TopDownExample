using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour, IMovable
{
    [SerializeField] private Transform _firePosition;
    [SerializeField] private float _speed;
    private Rigidbody2D _rigidbody2D;
    private Transform _target;
    private bool _find;
    private Vector2 _movePosition;
    private Vector2 _aimPosition;
    Stack<Vector2> _movingPosition = new Stack<Vector2>();
    private float _firerate = 0.5f;
    private float _nextShotTime ;
    float IMovable.Speed => _speed;
    Rigidbody2D IMovable.Rigidbody2D => _rigidbody2D;

    protected void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _target = FindObjectOfType<Player>().transform;
    }

    protected void Start()
    {
        StartCoroutine(CreateDirection());

    }

    private IEnumerator CreateDirection()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            _movePosition = new Vector2(transform.position.x/_target.position.x, transform.position.y/_target.position.y).normalized;
        }
    }
    
    protected void Update()
    {
        EventBus.RaiseEvent<IRenderTrajectory>(x=>x.ShowTrajectory(_firePosition));
        GenerateMovement();
    }


    protected void FixedUpdate()
    {
        if (A())
        {
            if (Time.time > _nextShotTime) 
            {
                new ShotCommand(_firePosition).Execute();
                _nextShotTime = Time.time + _firerate;
            }
        }


        else
        {
            //new MovementCommand(this, _movePosition).Execute();
            new AimCommand(this, Random.rotation.x).Execute();
        }
    }
    
    private bool A()
    {
        var rays = ReflectPoints.Reflect(_firePosition.position, _firePosition.up);
        if (rays.Count > 0)
        {
            var lastElement=rays.Last();
            var hit = Physics2D.OverlapPoint(lastElement.origin);
            if (hit != null)
            {
                if (hit.gameObject.layer == LayerMask.NameToLayer("Destroyable") && hit.CompareTag("Player"))
                    return true;
            }
        }
        return false;
    }

    private void GenerateMovement()
    {
       
        //_aimPosition = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;;
    }
}