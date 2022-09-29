using System;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryRender : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private List<Vector3> _points = new List<Vector3>();
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
        var queue = ReflectPoints.Reflect(startPos.position, startPos.up);
        _lineRenderer.positionCount = queue.Count+1;
        _lineRenderer.SetPosition(0,startPos.position);
        for (int i = 1; i <_lineRenderer.positionCount; i++)
        {
            var point = queue.Dequeue();
            _lineRenderer.SetPosition(i,point.origin);
        }
    }
    public void CleanupTrajectory()
    {
        _lineRenderer.positionCount = 0;
    }
}
