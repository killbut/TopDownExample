using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletTrajectoryRender : MonoBehaviour, IRenderTrajectory
{
    private LineRenderer _lineRenderer;
    private List<Vector3> _points = new List<Vector3>();
    private void Start()
    {
         _lineRenderer=GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe(this);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(this);
    }
    
    public void ShowTrajectory(Transform startPos)
    {
        var queue = ReflectPoints.Reflect(startPos.position, startPos.up);
        _lineRenderer.positionCount = queue.Count+1;
        _lineRenderer.SetPosition(0,startPos.position);
        for (int i = 1; i <_lineRenderer.positionCount; i++)
        {
            var point = queue.Dequeue();
            _lineRenderer.SetPosition(i,point.origin);
        }
    }
    // TODO if reflect to infinity space
    public void CleanupTrajectory()
    {
        _lineRenderer.positionCount = 0;
    }
}
