using System.Collections;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAI : Person
{
    [SerializeField] private float _distanceToPlayer;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _position;
    private Transform _transform;
    private Transform _target;
    private float _nextCyclePoint;
    private float _angle;

    protected void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _transform = transform;
        _target = FindObjectOfType<Player>().transform;
    }

    protected void Start()
    {
        base.Init();
        StartCoroutine(A());
    }

    private IEnumerator A()
    {
        while (true)
        {
            var targetPos = GetRandomNode();
            new Pathfinding(GridNode.Instance).FindPath(transform.position,targetPos);
            if(GridNode.Instance.Path!=null)
            {
                var path = GridNode.Instance.Path;
                for (int i = 0; i < path.Count; i++)
                {
                    var pos = path[i].WorldPosition;
                    new MovingCommand(pos*Setting.Speed*Time.fixedDeltaTime,_rigidbody2D).Execute();
                    yield return new WaitForFixedUpdate();
                }
            }
        }
    }

    private Vector2 GetRandomNode()
    {
        while (true)
        {
            var x = Random.Range(0, GridNode.Instance.SizeX);
            var y = Random.Range(0, GridNode.Instance.SizeY);
            var node = GridNode.Instance.Grid[x, y];
            if (node.Walkable)
                return node.WorldPosition;
        }
    }
    protected void FixedUpdate()
    {
       // if (CheckTrajectoryToPlayer())
           // new ShotCommand(Fireposition,Setting.Firerate);
       //
       // new AimCommand(_rigidbody2D, _angle).Execute();
    }
    private void CreatePointToMove()
    {
       
        // if (Vector2.Distance(_transform.position, _target.position) > _distanceToPlayer)
        // {
        //     var direction =  _target.position - _transform.position;
        //     _position = direction.normalized * (Setting.Speed * Time.fixedDeltaTime);
        //     _angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg-90f;
        // }
        // else
        // {
        //     _angle = 0;
        // }

    }
    private bool CheckTrajectoryToPlayer()
    {
        var rays = ReflectPoints.Reflect(transform.position, transform.up);
        if (rays.Count > 0)
        {
            var lastRay = rays.Last();
            if (Physics2D.OverlapPoint(lastRay.origin).CompareTag("Player"))
                return true;
        }
        return false;
    }
}