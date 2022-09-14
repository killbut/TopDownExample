using System;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private Transform _firePositionTransform;
    
    protected void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Player shoot");
            var bullet=Instantiate(_bullet, _firePositionTransform.position, _firePositionTransform.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(_firePositionTransform.up*_bulletSpeed,ForceMode2D.Impulse);
        }
    }
}