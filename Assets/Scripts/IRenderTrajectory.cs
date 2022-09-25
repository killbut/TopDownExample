using UnityEngine;

public interface IRenderTrajectory: IGlobalSubscriber
{
    void ShowTrajectory(Transform startPos);
    void CleanupTrajectory();

}