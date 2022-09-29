using UnityEngine;
public class AimCommand : ICommand
{
    private readonly Rigidbody2D _rigidbody2D;
    private readonly float _angle;

    public AimCommand(Rigidbody2D rigidbody2D, float angle)
    {
        _rigidbody2D = rigidbody2D;
        _angle = angle;
    }
    
    public void Execute()
    {
        if (_rigidbody2D != null)
        {
            _rigidbody2D.SetRotation(_angle);
            //_rigidbody2D.rotation = _angle;
        }
    }
}