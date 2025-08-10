using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectMR.Entities
{
    public class Entity : MonoBehaviour
    {
        [field: SerializeField] public LayerMask WhatIsTarget { get; private set; }
        [field: SerializeField] public float TargetDetectRange { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }
        [SerializeField] private bool _showRange;

        public bool IsTargetInRange(float range, out Collider2D collider)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range, WhatIsTarget);
            collider = null;
            float minDistance = float.MaxValue;
            for (int i = 0; i < colliders.Length; i++)
            {
                float targetDistance = Vector2.Distance(colliders[i].transform.position, transform.position);
                if (targetDistance < minDistance)
                {
                    collider = colliders[i];
                    minDistance = targetDistance;
                }
            }
            return colliders.Length > 0;
        }

        public virtual void OnDie()
        {

        }

        protected Dictionary<Type, IEntityComponent> _components;

        protected virtual void Awake()
        {
            FindComponents();
            InitComponents();
            AfterInitComponents();
        }

        protected virtual void FindComponents()
        {
            _components = new Dictionary<Type, IEntityComponent>();
            GetComponentsInChildren<IEntityComponent>(true).ToList()
                .ForEach(component => _components.Add(component.GetType(), component));
        }

        protected virtual void InitComponents()
        {
            _components.Values.ToList().ForEach(component => component.Initialize(this));
        }

        protected virtual void AfterInitComponents()
        {
            _components.Values.ToList().ForEach(component =>
            {
                if (component is IAfterInitComponent afterInitable)
                {
                    afterInitable.AfterInit();
                }
            });
        }

        protected virtual void DisposeComponents()
        {
            _components.Values.ToList().ForEach(component =>
            {
                if (component is IAfterInitComponent disposeable)
                {
                    disposeable.Dispose();
                }
            });
        }

        public T GetCompo<T>(bool isDerived = false) where T : IEntityComponent
        {
            if (_components.TryGetValue(typeof(T), out IEntityComponent component))
            {
                return (T)component;
            }

            if (isDerived == false)
                return default;

            Type findType = _components.Keys.FirstOrDefault(t => t.IsSubclassOf(typeof(T)));
            if (findType != null)
                return (T)_components[findType];

            return default;
        }

        protected virtual void OnDestroy()
        {
            DisposeComponents();
        }

        private void OnDrawGizmos()
        {
            if (_showRange)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, TargetDetectRange);
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, AttackRange);
            }
        }
    }
}
