using UnityEngine;
public class AimCommand : ICommand
{
    private readonly Rigidbody2D _rigidbody2D;
    private readonly float _angle;

    public AimCommand(IMovable movement, float angle)
    {
        _rigidbody2D = movement.Rigidbody2D;
        _angle = angle;
    }
    
    public void Execute()
    {
        if (_rigidbody2D != null)
        {
            _rigidbody2D.rotation = _angle;
        }
    }
}