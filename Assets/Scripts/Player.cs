using UnityEngine;
public class Player :  MonoBehaviour, IMovable
{
    [SerializeField] private Transform _firePosition;
    [SerializeField] private float _speed;
    float IMovable.Speed => _speed;
    Rigidbody2D IMovable.Rigidbody2D=> _rigidbody2D;

    private Vector2 _movementVector;
    private Vector2 _mousePosition;
    private Camera _camera;
    private Rigidbody2D _rigidbody2D;
    private float _angle;

    private void Start()
    {
        _camera=Camera.main;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    protected void Update()
    {
        if(Input.GetMouseButtonDown(0))
            new ShotCommand(_firePosition).Execute();
        if(Input.GetMouseButton(1))
            EventBus.RaiseEvent<IRenderTrajectory>(x=>x.ShowTrajectory(_firePosition));
        if(Input.GetMouseButtonUp(1))
            EventBus.RaiseEvent<IRenderTrajectory>(x=>x.CleanupTrajectory());
        
        _movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        var lookingDirection = _mousePosition - _rigidbody2D.position;
        _angle = Mathf.Atan2(lookingDirection.y, lookingDirection.x) * Mathf.Rad2Deg - 90f;
    }

    protected void FixedUpdate()
    {
        new MovementCommand(this, _movementVector).Execute();
        new AimCommand(this,_angle).Execute();
    }



}