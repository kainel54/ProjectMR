using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProjectMR.Core.Pool
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        private Dictionary<PoolingKey, Pool<IPoolable>> _poolDictionary
        = new Dictionary<PoolingKey, Pool<IPoolable>>();

        public PoolListSO poolListSO;

        protected override void FirstInitialize()
        {
            base.FirstInitialize();
            foreach (PoolingItemSO item in poolListSO.GetList())
            {
                CreatePool(item);
            }
        }

        private void CreatePool(PoolingItemSO item)
        {
            var key = item.PoolObj.PoolEnum;
            var pool = new Pool<IPoolable>(item.PoolObj, new PoolingKey(key), transform, item.poolCount);
            _poolDictionary.Add(new PoolingKey(key), pool);
        }

        public IPoolable Pop(Enum type)
        {
            PoolingKey key = new PoolingKey(type);

            if (!_poolDictionary.ContainsKey(key))
            {
                Debug.LogError($"Prefab does not exist on pool : {key}");
                return null;
            }

            var item = _poolDictionary[key].Pop();
            item.OnPop();
            return item;
        }

        public void Push(IPoolable obj, bool resetParent = false)
        {
            if (resetParent)
                obj.GameObject.transform.SetParent(transform);
            obj.OnPush();
            _poolDictionary[new PoolingKey(obj.PoolEnum)].Push(obj);
        }
    }

    public static class PoolingUtility
    {
        public static IPoolable Pop(this GameObject gameObject, Enum type)
        {
            if (PoolManager.Instance == null)
            {
                Debug.LogError("PoolManager가 없습니다.");
                return null;
            }
            else
            {
                IPoolable poolable = PoolManager.Instance.Pop(type);
                GameObject obj = poolable.GameObject;
                obj.transform.parent = null;
                obj.transform.position = Vector3.zero;
                return poolable;
            }
        }
        public static IPoolable Pop(this GameObject gameObject, Enum type, Transform parent)
        {
            if (PoolManager.Instance == null)
            {
                Debug.LogError("PoolManager가 없습니다.");
                return null;
            }
            else
            {
                IPoolable poolable = PoolManager.Instance.Pop(type);
                GameObject obj = poolable.GameObject;
                obj.transform.parent = parent;
                obj.transform.position = Vector3.zero;
                return poolable;
            }
        }
        public static IPoolable Pop(this GameObject gameObject, Enum type, Vector3 position, Quaternion rotation)
        {
            if (PoolManager.Instance == null)
            {
                Debug.LogError("PoolManager가 없습니다.");
                return null;
            }
            else
            {
                IPoolable poolable = PoolManager.Instance.Pop(type);
                GameObject obj = poolable.GameObject;
                obj.transform.parent = null;
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                return poolable;
            }
        }
        public static void Push(this IPoolable poolable)
        {
            PoolManager poolManager = PoolManager.Instance;
            if (poolManager == null)
            {
                Debug.LogError("PoolManager가 없습니다.");
            }
            else
            {
                GameObject obj = poolable.GameObject;
                obj.transform.parent = poolManager.transform;
                obj.transform.position = Vector3.zero;
                obj.transform.rotation = Quaternion.identity;
                poolManager.Push(poolable);
            }
        }
    }
}



