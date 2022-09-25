using UnityEngine;
public class RotatingCommand : ICommand
{
    private readonly Rigidbody2D _rigidbody2D;
    private readonly Vector2 _mousePosition;

    public RotatingCommand(Movement movement, Vector2 mousePosition)
    {
        _rigidbody2D = movement.Rigidbody2D;
        _mousePosition = mousePosition;
    }
    
    public void Execute()
    {
        if (_rigidbody2D != null)
        {
            var lookingDirection = _mousePosition - _rigidbody2D.position;
            float angle = Mathf.Atan2(lookingDirection.y, lookingDirection.x) * Mathf.Rad2Deg - 90f;
            _rigidbody2D.rotation = angle;
        }
    }
}