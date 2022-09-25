using UnityEngine;

public class ShotCommand : ICommand
{
    private readonly Transform _firePosition;
    
    public ShotCommand(Transform firePosition)
    {
        _firePosition = firePosition;
    }
    public void Execute()
    { 
        EventBus.RaiseEvent<IBulletPoolObjectHandler>(x => x.TakeFreeBullet().Shot(_firePosition));
    }
}