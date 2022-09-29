using UnityEngine;

public class TrajectoryCommand : ICommand
{
    private readonly Transform _startPosition;
    private readonly bool _needClearTrajectory;
    public TrajectoryCommand(Transform startPosition,bool clear)
    {
        _startPosition = startPosition;
        _needClearTrajectory = clear;
    }
    public void Execute()
    {
        if(!_needClearTrajectory)
            TrajectoryRender.Instance.ShowTrajectory(_startPosition);
        else
            TrajectoryRender.Instance.CleanupTrajectory();
    }
}