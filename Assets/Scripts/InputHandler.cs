using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]

public class InputHandler : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private CircleCollider2D _circleCollider2D;
    private Camera _camera;
    private float _rotationAngle;
    private Vector2 _direction;
    private Transform _fireposition;
    private SettingPerson _setting;
    private float _nextShotTime;
    
    private bool _isShot;
    private bool _isTrajectory;
    private float _cycleShot;

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
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _camera = Camera.main;
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
            new TrajectoryCommand(_fireposition,true).Execute();
            _isTrajectory = false;
        }
        CalculateAngle();
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
            new TrajectoryCommand(_fireposition,false).Execute();
        }
        new MovingCommand(_direction, _rigidbody2D).Execute();
        new AimCommand(_rigidbody2D, _rotationAngle).Execute();
    }

    private void CalculateAngle()
    {
        var mousePosition =(Vector2) _camera.ScreenToWorldPoint(Input.mousePosition);
        var lookingDirection = mousePosition - _rigidbody2D.position;
        _rotationAngle = Mathf.Atan2(lookingDirection.y, lookingDirection.x) * Mathf.Rad2Deg - 90f;
    }

    private void CalculateDirection()
    {
        var direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _direction = direction * (_setting.Speed * Time.fixedDeltaTime);
        //_direction =(Vector2) transform.position +  direction * (_setting.Speed * Time.fixedDeltaTime);
    }
}