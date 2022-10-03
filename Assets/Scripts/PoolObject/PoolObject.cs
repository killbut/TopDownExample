using System;
using System.Collections.Generic;
using UnityEngine;

namespace PoolObject
{
    public class PoolObject<T> where T : MonoBehaviour
    {
        private T _prefab;
        private bool _autoExpand;
        private Transform _container;
        private List<T> _poolObjectList;

        public List<T> PoolObjectList => _poolObjectList;
        
        public PoolObject(T prefab,bool autoExpand, Transform container,int startSize=10)
        {
            _prefab = prefab;
            _autoExpand = autoExpand;
            _container = container;
            CreatePoolList(startSize);
        }

        private void CreatePoolList(int size)
        {
            _poolObjectList = new List<T>();
            for (int i = 0; i < size; i++) 
                CreatePrefabObject();
            Debug.Log("CreatePoolList() Done");
        }

        private T CreatePrefabObject(bool setActiveDefault=false)
        {
            var objectForPool = GameObject.Instantiate(_prefab, _container);
            objectForPool.gameObject.SetActive(setActiveDefault);
            _poolObjectList.Add(objectForPool);
            return objectForPool;
        }

        public bool HasFreeObject(out T gameObject)
        {
            foreach (var item in _poolObjectList)
            {
                if (!item.gameObject.activeInHierarchy)
                {
                    gameObject = item;
                    return true;
                }
            }

            gameObject = null;
            return false;
        }

        public T GetFreeObject()
        {
            if (HasFreeObject(out var gameObject))
            {
                gameObject.gameObject.SetActive(true);
                return gameObject;
            }

            if (_autoExpand)
            {
                return CreatePrefabObject(true);
            }
                
            throw new ArgumentException("No free element in ObjectPool");
        }

        public void ReturnAllFreeObject()
        {
            foreach (var item in _poolObjectList)
            {
                if(item.gameObject.activeInHierarchy)
                    item.gameObject.SetActive(false);
            }
        }
    }
}
