using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]

public class InputHandler : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private Camera _camera;
    private float _rotationAngle;
    private Vector2 _movingDirection;
    private Transform _fireposition;
    private Persons _setting;
    private float _nextShotTime;
    
    private bool _isShot;
    private bool _isTrajectory;
    private float _cycleShot;
    private Vector2 _rotatingDirection;
    private TrajectoryRender _trajectoryInstance;

    private void Awake()
    {
        CacheFields();
    }

    private void CacheFields()
    {
        var player = GetComponent<Player>();
        _fireposition = player.Fireposition;
        _setting = player.Setting;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
        _trajectoryInstance = FindObjectOfType<TrajectoryRender>();
    }

    protected void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_isShot)
        {
            _isShot = true;
        }
        if (Input.GetMouseButton(1) && !_isTrajectory)
        {
            _isTrajectory = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            new TrajectoryCommand(_trajectoryInstance,_fireposition,true).Execute();
            _isTrajectory = false;
        }
        CalculateDirection();
    }

    protected void FixedUpdate()
    {
        if (_isShot)
        {
            if (Time.time > _cycleShot)
            {
                _cycleShot = Time.time + _setting.Firerate;
                new ShotCommand(_fireposition).Execute();
                _isShot = false;
            }
        }
        if (_isTrajectory)
        {
            new TrajectoryCommand(_trajectoryInstance,_fireposition,false).Execute();
        }
        new MovingCommand(_rigidbody2D,_movingDirection).Execute();
        new AimCommand(_rigidbody2D,_rotatingDirection).Execute();
    }
    
    private void CalculateDirection()
    {
        var direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _movingDirection = direction * (_setting.Speed * Time.fixedDeltaTime);
        _rotatingDirection =(Vector2) _camera.ScreenToWorldPoint(Input.mousePosition) -_rigidbody2D.position;
    }
}