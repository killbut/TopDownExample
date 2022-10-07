using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : Person
{
    private const float DELTA_DISTANCE = 0.2f;

    private Pathfinding _pathfinding;
    private Transform _target;

    private Rigidbody2D _rigidbody2D;
    private bool _needPath=true;
    private float _nextCycleShot;
    private float _nextCheckStuck;

    protected void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _pathfinding = FindObjectOfType<Pathfinding>();
    }

    protected void Start()
    {
        base.Init();
        _target = FindObjectOfType<Player>().transform;
    }

    private void OnEnable()
    {
        _pathfinding.OnPathfinded += GetPath;
        StartCoroutine(CreatePath());

    }

    private void OnDisable()
    {
        _pathfinding.OnPathfinded -= GetPath;
    }

    private void GetPath(Vector2[] path)
    {
        _needPath = false;
        StartCoroutine(MovingToPath(path));
    }

    private IEnumerator CreatePath()
    {
        while (true)
        {
            if (!_needPath)
            {
                yield return null;
            }
            else if(_target!=null)
            {
                _pathfinding.FindPath(transform.position, _target.position);
                yield return new WaitForSeconds(3f);
            }

            yield return new WaitUntil(() => _target != null);
        }
    }
    
    private IEnumerator MovingToPath(Vector2[] path)
    {
        int index = 0;
        Vector2 currentWaypoint = path[index];
        while (true)
        {
            
            if (Vector2.Distance(transform.position, currentWaypoint) < DELTA_DISTANCE)
            {
                index++;
                if (index >= path.Length)
                {
                    _needPath = true;
                    yield break;
                }

                currentWaypoint = path[index];
            }

            var moveTowards = Vector2.MoveTowards(_rigidbody2D.position, currentWaypoint,
                Setting.Speed * Time.fixedDeltaTime);
            _rigidbody2D.MovePosition(moveTowards); // TODO spline path

            var direction = (_target.position-transform.position);
            var angleRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg-90f;
            _rigidbody2D.SetRotation(angleRotation);
            yield return new WaitForFixedUpdate();
        }
    }

    
    private void FixedUpdate()
    {
        if (_rigidbody2D.velocity == Vector2.zero)
            _needPath = true;
        if (TrajectoryToPlayer())
        {
            if (Time.time > _nextCycleShot)
            {
                new ShotCommand(Fireposition).Execute();
                _nextCycleShot = Time.time + Setting.Firerate;
            }
        }
    }

    private bool TrajectoryToPlayer()
    {
        var rays = new ReflectPoints(Fireposition.position, Fireposition.up).Reflect();
        var lastRay = rays.Last();
        var hit = Physics2D.OverlapCircle(lastRay.origin, 0.3f);
        if (hit != null)
            if (hit.gameObject.CompareTag("Player"))
                return true;
        return false;
    }
    
}