using UnityEngine;

public class AimCommand : ICommand
{
    private readonly Rigidbody2D _rigidbody2D;
    private readonly Vector2 _direction;

    public AimCommand(Rigidbody2D rigidbody2D, Vector2 direction)
    {
        _rigidbody2D = rigidbody2D;
        _direction = direction;
    }

    public void Execute()
    {
        var angle= Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90f;
        _rigidbody2D.SetRotation(angle);
    }
}