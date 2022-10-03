using UnityEngine;

namespace PoolObject
{
     public class PoolManager : MonoBehaviour
     {
          [SerializeField] private Bullet _prefab;
          [SerializeField] private int _startSize = 10;
          [SerializeField] private bool _needAutoExpand = false;
          [SerializeField] private Transform _container;

          private PoolObject<Bullet> _bulletPoolObject;
          private static PoolManager _instance;

          public static PoolManager Instance => _instance;
     

          private void Awake()
          {
               _instance = this;
          }

          protected void Start()
          {
               _bulletPoolObject = new PoolObject<Bullet>(_prefab, _needAutoExpand, _container, _startSize);
          }
     
          public Bullet TakeFreeBullet()
          {
               var bullet = _bulletPoolObject.GetFreeObject();
               return bullet;
          }
     }
}
