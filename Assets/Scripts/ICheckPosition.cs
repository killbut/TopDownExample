using UnityEngine;

public interface ICheckPosition : IGlobalSubscriber
{
    Vector2 CheckPosition(Vector2 position);
}