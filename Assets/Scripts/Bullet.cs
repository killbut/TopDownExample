using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;

    private Rigidbody2D _rigidbody2D;
    public Rigidbody2D Rigidbody2D => _rigidbody2D;
    public float Speed => _speed;
    protected void Awake()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        Debug.Log("Bullet initialization");
    }
    protected void OnEnable()
    {
        
    }

    protected void OnDisable()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("OnCollisionEnter2D");
    }

    protected void OnBecameInvisible()
    {
        Debug.Log("OnBecameInvisible");
        EventBus.RaiseEvent<IBulletPoolObjectHandler>(x=>x.DeactivateBullet(this.gameObject));
    }
    
}