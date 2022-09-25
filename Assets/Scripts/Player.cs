using System;
using UnityEngine;
public class Player :  Movement
{
    [SerializeField] private Transform _firePosition;
    
    private Vector2 _movementVector;
    private Vector2 _mousePosition;
    private Camera _camera;

    private void Start()
    {
        _camera=Camera.main;
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }
    protected void Update()
    {
        if(Input.GetMouseButtonDown(0))
            new ShotCommand(_firePosition).Execute();
        if(Input.GetMouseButtonDown(1))
            EventBus.RaiseEvent<IRenderTrajectory>(x=>x.ShowTrajectory(_firePosition));
        _movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
    }

    protected void FixedUpdate()
    {
        new MovementCommand(this, _movementVector).Execute();
        new RotatingCommand(this,_mousePosition).Execute();
    }


}