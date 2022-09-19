using System;
using UnityEngine;

public class BulletTrajectoryRender : MonoBehaviour
{
    [SerializeField] private Transform _startTransform;
    
    private LineRenderer _lineRenderer;
    private int _maxCountReflect = 25;
    private Vector3[] _pointsReflect;
    private void Start()
    {
         _lineRenderer=GetComponent<LineRenderer>();
         _pointsReflect = new Vector3[_maxCountReflect];
         _lineRenderer.positionCount = _maxCountReflect;
    }

    public void ShowTrajectory()
    {
        _lineRenderer.SetPositions(_pointsReflect);
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            ReflectRay(_startTransform.position,_startTransform.up,0);
            ShowTrajectory();
        }
    }

    private void ReflectRay(Vector2 startPos,Vector2 direction,int count)
    {
        Ray2D ray = new Ray2D(startPos, direction);
        var hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit.collider.CompareTag("Wall") && count<_maxCountReflect)
        {
            _pointsReflect[count] = ray.origin;
            count++;
            // TODO fix delta
            ray.origin = hit.point - ray.direction.normalized;
            
            ray.direction = Vector2.Reflect(ray.direction, hit.normal);
            ReflectRay(ray.origin,ray.direction ,count);
        }
    }
}
