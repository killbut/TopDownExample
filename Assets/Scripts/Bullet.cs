using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    
    private Rigidbody2D _rigidbody2D;
    private Queue<Ray2D> _reflectPoints;
    private Vector2 _lastFrameVelocity;
    public Rigidbody2D Rigidbody2D => _rigidbody2D;
    public float Speed => _speed;
        
    protected void Awake()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _lastFrameVelocity = _rigidbody2D.velocity;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            ReflectBullet();
        }
           
        if (col.gameObject.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
        }
            
        if(col.gameObject.CompareTag("Bullet"))
            gameObject.SetActive(false);
    }
    
    protected void OnBecameInvisible()
    {
        this.gameObject.SetActive(false);
    }

    public void Shot(Transform firePosition)
    {
        transform.position = firePosition.position;
        transform.rotation = firePosition.rotation;
        _rigidbody2D.velocity = firePosition.up *_speed;
        _reflectPoints=ReflectTrajectory.Reflect(firePosition.position, firePosition.up);
    }

    private void ReflectBullet()
    {
        if (_reflectPoints.TryDequeue(out var ray))
        {
            _rigidbody2D.velocity = ray.direction * _speed;
            _rigidbody2D.position = ray.origin;
            var angle = Mathf.Atan2(ray.direction.y ,ray.direction.x) * Mathf.Rad2Deg-90f;
            _rigidbody2D.rotation = angle;
            Debug.Log("Pos:"+ray.origin);
        }
    }
    
}