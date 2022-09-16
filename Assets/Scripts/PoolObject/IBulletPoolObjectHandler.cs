using UnityEngine;

public interface IBulletPoolObjectHandler : IGlobalSubscriber
{
    void DeactivateBullet(GameObject bullet);
    void ShootBullet(Transform firePosition);
}