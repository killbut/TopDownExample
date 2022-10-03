using UnityEngine;

public class TrajectoryCommand : ICommand
{
    private readonly Transform _startPosition;
    private readonly bool _needClearTrajectory;
    private readonly TrajectoryRender _instance;
    public TrajectoryCommand(TrajectoryRender instance,Transform startPosition,bool clear)
    {
        _startPosition = startPosition;
        _needClearTrajectory = clear;
        _instance = instance;
    }
    public void Execute()
    {
        if (!_needClearTrajectory)
            _instance.ShowTrajectory(_startPosition);
        else
            _instance.CleanupTrajectory();
    }
}