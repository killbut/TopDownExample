using UnityEngine;

public class ShotCommand : ICommand
{
    private readonly Transform _firePosition;

    private float _cycleShot;
    
    public ShotCommand(Transform firePosition)
    {
        _firePosition = firePosition;
    }
    public void Execute()
    {
        var bullet=PoolManager.Instance.TakeFreeBullet();
        bullet.Shot(_firePosition);
    }
}