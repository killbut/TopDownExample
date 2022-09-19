using UnityEngine;
using Vector2 = UnityEngine.Vector2;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;

    private Rigidbody2D _rigidbody2D;
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
            ReflectBullet(col.GetContact(0).normal);
    }

    protected void OnBecameInvisible()
    {
        this.gameObject.SetActive(false);
    }

    public void Shot(Transform firePosition)
    {
        transform.position = firePosition.position;
        transform.rotation = firePosition.rotation;
        _rigidbody2D.AddForce(firePosition.up * Speed, ForceMode2D.Impulse);
    }

    private void ReflectBullet(Vector2 inNormal)
    {
        var newDirection = Vector2.Reflect(_lastFrameVelocity.normalized, inNormal);
        var angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg - 90f;
        _rigidbody2D.velocity = newDirection * _speed;
        _rigidbody2D.rotation = angle;
    }
    
}