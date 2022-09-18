using UnityEngine;

public interface IBulletPoolObjectHandler : IGlobalSubscriber
{
    void Shot(Transform firePosition);
}