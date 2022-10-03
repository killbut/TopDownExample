using UnityEngine;

public class TrajectoryRender : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private static TrajectoryRender _instance;
    public static TrajectoryRender Instance => _instance;

    protected void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _lineRenderer=GetComponent<LineRenderer>();
    }
    
    public void ShowTrajectory(Transform startPos)
    {
        var rays = new ReflectPoints(startPos.position, startPos.up).Reflect();
        _lineRenderer.positionCount = rays.Count+1;
        _lineRenderer.SetPosition(0,startPos.position);
        int index = 1;
        foreach (var ray in rays)
        {
            _lineRenderer.SetPosition(index,ray.origin);
            index++;
        }
    }
    public void CleanupTrajectory()
    {
        _lineRenderer.positionCount = 0;
    }
}
