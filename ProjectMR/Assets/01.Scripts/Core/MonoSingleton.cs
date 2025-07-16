using System;
using UnityEngine;

namespace ProjectMR.Core
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _Instance;
        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = FindFirstObjectByType<T>();
                    if (_Instance == null)
                    {
                        //없으면 만들기
                        Type singleton = typeof(T);
                        GameObject singletonObj = new GameObject($"{singleton.ToString()}", singleton);
                        _Instance = singletonObj.GetComponent<T>();
                    }
                    if (_Instance._isInitialized == false)
                        _Instance.FirstInitialize();
                }
                return _Instance;
            }
        }

        private bool _isInitialized;

        protected virtual void Awake()
        {
            if (_isInitialized) return;
            FirstInitialize();
        }

        protected virtual void FirstInitialize() 
        {
            _isInitialized = true;
        }
    }
}