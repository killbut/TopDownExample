using UnityEngine;

public interface IBulletPoolObjectHandler : IGlobalSubscriber
{
    Bullet TakeFreeBullet();
}