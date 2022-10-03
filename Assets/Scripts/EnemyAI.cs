using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : Person
{
    private const float DELTA_POSITIONS = 0.2f;
    private Rigidbody2D _rigidbody2D;
    private GridNodes _grid;
    private Transform _transform;
    private Vector2 _endPoint;
    private Vector2[] _path;
    private bool _needPath;
    private Ray2D[] _rays;
    private float _nextCycleShot;

    protected void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _grid = FindObjectOfType<GridNodes>();
        _transform = transform;
    }

    protected void Start()
    {
        base.Init();
        GetPath();
        StartCoroutine(MovingToPath());
    }

    private void GetPath()
    {
        var randomNode = _grid.GetRandomNode();
        _path = _grid.Pathfinding.FindPath(_transform.position, randomNode.WorldPosition);
    }

    private IEnumerator MovingToPath()
    {
        int index = 0;
        Vector2 currentWaypoint = _path[index];
        while (true)
        {
            if (Vector2.Distance(_transform.position, currentWaypoint) < DELTA_POSITIONS)
            {
                index++;
                if (index >= _path.Length)
                {
                    index = 0;
                    GetPath();
                    yield return null;
                    continue;
                }

                currentWaypoint = _path[index];
                
            }

            var moveTowards = Vector2.MoveTowards(_rigidbody2D.position, currentWaypoint,
                Setting.Speed * Time.fixedDeltaTime);
            _rigidbody2D.MovePosition(moveTowards); // TODO spline path

            var direction = (currentWaypoint - _rigidbody2D.position).normalized;
            var rotateTowards = Vector3.RotateTowards(_transform.up, direction, 3 * Time.fixedDeltaTime, 0);
            _rigidbody2D.SetRotation(Quaternion.LookRotation(_transform.forward, rotateTowards));

            yield return new WaitForFixedUpdate();
        }
    }

    private void FixedUpdate()
    {
        var rays = new ReflectPoints(Fireposition.position, Fireposition.up).Reflect();
        var lastRay = rays.Last();
        var hit = Physics2D.OverlapCircle(lastRay.origin, 0.3f);
        if (hit != null)
            if (hit.gameObject.CompareTag("Player"))
            {
                if (Time.time > _nextCycleShot)
                {
                    _nextCycleShot = Time.time + Setting.Firerate;
                    new ShotCommand(Fireposition).Execute();
                }
            }
    }
}