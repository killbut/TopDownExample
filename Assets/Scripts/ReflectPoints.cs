using System.Collections.Generic;
using UnityEngine;

public class ReflectPoints
{
    private const int MAX_REFLECTION = 100;
    private const int RAY_LENGHT = 50;
    public static Queue<Ray2D> Reflect(Vector2 startPos, Vector2 directionPos)
    {
        Queue<Ray2D> rays = new Queue<Ray2D>();
        Ray2D ray = new Ray2D(startPos,directionPos);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        for (int count = 0; count < MAX_REFLECTION ;count++)
        {
            if (hit.collider != null)
            {
                if (hit.transform.gameObject.layer==LayerMask.NameToLayer("Walls"))
                {
                    ray.direction = Vector2.Reflect(ray.direction, hit.normal);
                    ray.origin = hit.point + ray.direction * 0.1f;
                    rays.Enqueue(ray);
                    ray = new Ray2D(ray.origin, ray.direction);
                    hit = Physics2D.Raycast(ray.origin, ray.direction);
                }
                else if (hit.transform.gameObject.layer==LayerMask.NameToLayer("Destroyable"))
                {
                    ray.origin = hit.point;
                    rays.Enqueue(ray);
                    return rays;
                }
            }
            else
            {
                ray.origin += ray.direction * RAY_LENGHT;
                rays.Enqueue(ray);
                return rays;
            }
        }
        return rays;
    }

}