using UnityEngine;
public class MovementCommand : ICommand
{
    private readonly Rigidbody2D _rigidbody2D;
    private readonly float _speed;
    private readonly Vector2 _position;
    
    public MovementCommand(Movement movement, Vector2 position)
    {
        _rigidbody2D = movement.Rigidbody2D;
        _speed = movement.Speed;
        _position = position;
    }
    public void Execute()
    {
        if (_rigidbody2D != null)
        {
            _rigidbody2D.MovePosition(_rigidbody2D.position+_position * (_speed * Time.fixedDeltaTime));
        }
    }
}