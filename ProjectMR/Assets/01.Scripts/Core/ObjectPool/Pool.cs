using System.Collections.Generic;
using UnityEngine;

namespace ProjectMR.Core.Pool
{
    public class Pool<T> where T : IPoolable
    {
        private Stack<T> _pool = new Stack<T>();
        private T _prefab;
        private Transform _parent;

        private PoolingKey _type;

        public Pool(T prefab, PoolingKey type, Transform parent, int count)
        {
            _prefab = prefab;
            _type = type;
            _parent = parent;

            for(int i = 0; i < count; i++)
            {
                T obj = GameObject.Instantiate(_prefab.GameObject, _parent).transform.GetComponent<T>();
                obj.GameObject.name = _type.ToString();
                obj.GameObject.SetActive(false);
                _pool.Push(obj);
            }
        }


        public T Pop()
        {
            T obj = default;
            if(_pool.Count <= 0)
            {
                obj = GameObject.Instantiate(_prefab.GameObject, _parent).transform.GetComponent<T>(); ;
                obj.GameObject.name = _type.ToString();
            }
            else
            {
                obj = _pool.Pop(); 
                obj.GameObject.SetActive(true);
            }
            return obj;
        }

        public void Push(T obj)
        {
            obj.GameObject.SetActive(false);
            _pool.Push(obj);
        }
    }
}
