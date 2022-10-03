﻿using UnityEngine;

public class MovingCommand : ICommand
{
    private readonly Rigidbody2D _rigidbody2D;
    private Vector2 _direction;
    public MovingCommand(Rigidbody2D rigidbody2D,Vector2 direction)
    {
        _rigidbody2D = rigidbody2D;
        _direction = direction;
    }

    public void Execute()
    {
        _rigidbody2D.MovePosition(HoldingPosition.Instance.CheckPosition(_rigidbody2D.position+_direction));
    }
}