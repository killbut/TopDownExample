using UnityEngine;

public interface IBulletPoolObjectHandler : IGlobalSubscriber
{
    void TakeFreeBullet(Transform firePosition);
}