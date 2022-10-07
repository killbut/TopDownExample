using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Rigidbody2D _rigidbody2D;
    private Queue<Ray2D> _reflectPoints;
    public static event Action<GameObject> OnHitPerson;
    protected void Awake()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Walls"))
            ReflectBullet(col.contacts[0].normal);
        else if (col.gameObject.layer == LayerMask.NameToLayer("Destroyable"))
        {
            gameObject.SetActive(false);
            OnHitPerson?.Invoke(col.gameObject);
        }
    }
    
    protected void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
    
    public void Shot(Transform firePosition)
    {
        transform.position = firePosition.position;
        transform.rotation = firePosition.rotation;
        _rigidbody2D.velocity = firePosition.up * _speed;
        //_reflectPoints = new ReflectPoints(firePosition.position, firePosition.up).Reflect(true);
    }

    private void ReflectBullet(Vector2 normal)
    {
        _rigidbody2D.velocity = Vector2.Reflect(transform.up, normal) * _speed;
        var angle = Mathf.Atan2(_rigidbody2D.velocity.y, _rigidbody2D.velocity.x) * Mathf.Rad2Deg - 90f;
        _rigidbody2D.SetRotation(angle);
        // if (_reflectPoints.TryDequeue(out var ray)) //todo need determination behavior  
        // {
        //     _rigidbody2D.velocity = ray.direction * _speed;
        //     //_rigidbody2D.position = ray.origin;
        //     var angle = Mathf.Atan2(ray.direction.y, ray.direction.x) * Mathf.Rad2Deg - 90f;
        //     _rigidbody2D.rotation = angle;
        // }
    }
}