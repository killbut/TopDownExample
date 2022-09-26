using UnityEngine;

public interface IMovable
{
    float Speed { get; }
    Rigidbody2D Rigidbody2D { get;  }
}