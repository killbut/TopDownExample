using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    [SerializeField] private float _speed;
    public float Speed => _speed;
    public Rigidbody2D Rigidbody2D { get; protected  set; }
    

}