using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Rigidbody2D _rigidbody2D;
    private Queue<Ray2D> _reflectPoints;
    private bool _needReflectPoints = true;
    public Rigidbody2D Rigidbody2D => _rigidbody2D;
    public float Speed => _speed;

    protected void Awake()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Walls"))
            ReflectBullet();
        else if (col.gameObject.layer == LayerMask.NameToLayer("Destroyable")) col.gameObject.SetActive(false);
    }

    protected void OnBecameInvisible()
    {
        this.gameObject.SetActive(false);
        _needReflectPoints = true;
    }

    public void Shot(Transform firePosition)
    {
        transform.position = firePosition.position;
        transform.rotation = firePosition.rotation;
        _rigidbody2D.velocity = firePosition.up * _speed;
        if (_needReflectPoints)
        {
            _reflectPoints = new ReflectPoints(firePosition.position, firePosition.up).Reflect();
            _needReflectPoints = false;
        }
    }

    private void ReflectBullet()
    {
        if (_reflectPoints.TryDequeue(out var ray))
        {
            _rigidbody2D.velocity = ray.direction * _speed;
            _rigidbody2D.position = ray.origin;
            var angle = Mathf.Atan2(ray.direction.y, ray.direction.x) * Mathf.Rad2Deg - 90f;
            _rigidbody2D.rotation = angle;
        }
    }

    private void OnDrawGizmos()
    {
        if (_reflectPoints != null)
            foreach (var ray in _reflectPoints)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawRay(ray.origin, ray.direction * 10f);
            }
    }
}