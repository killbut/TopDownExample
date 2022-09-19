using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour 
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _firePosition;

    private Rigidbody2D _rigidbody2D;
    private Vector2 _movementVector;
    private Camera _camera;
    private Vector2 _mousePosition;

    protected void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
    }
    
    protected void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            EventBus.RaiseEvent<IBulletPoolObjectHandler>(x => x.TakeFreeBullet(_firePosition));
        }

        if (Input.GetMouseButton(1))
        {
            
        }
        GetInputPosition();
        _mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
    }

    protected void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
    }

    private void GetInputPosition()
    {
        _movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void MovePlayer()
    {
        _rigidbody2D.MovePosition(_rigidbody2D.position + _movementVector * (_speed * Time.fixedDeltaTime));
    }

    private void RotatePlayer()
    {
        var lookingDirection = _mousePosition - _rigidbody2D.position;
        float angle = Mathf.Atan2(lookingDirection.y, lookingDirection.x) * Mathf.Rad2Deg - 90f;
        _rigidbody2D.rotation = angle;
    }
    
}