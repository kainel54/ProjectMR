using ProjectMR.Core;
using ProjectMR.Stat;
using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

namespace ProjectMR.Entity
{
    public class EntityHealth : MonoBehaviour,IEntityComponent,IAfterInitComponent
    {
        [SerializeField] private DamageText _damageText;
        public BigInteger MaxHealthBigInteger => _maxHealthStat.BigIntValue;
        public BigInteger CurrentHealthBigInteger { get; private set; }
        public event Action OnDeadEvent;
        public bool IsDead { get; private set; }
        private StatElement _maxHealthStat;

        public event Action<BigInteger, BigInteger> OnHealthChangedEvent;

        private Entity _entity;

        private List<DamageText> _damageTextList = new List<DamageText>();

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        public void AfterInit()
        {
            _maxHealthStat = _entity.GetCompo<EntityStat>().StatDictionary[StatName.MaxHealth];
            CurrentHealthBigInteger = _maxHealthStat.BigIntValue;
            OnHealthChangedEvent?.Invoke(_maxHealthStat.BigIntValue, _maxHealthStat.BigIntValue);
            IsDead = false;
        }

        public void Dispose()
        {
            IsDead = true;
        }

        public void ApplyDamage(BigInteger bigInteger)
        {
            if (IsDead) return;

            BigInteger prev = CurrentHealthBigInteger;
            CurrentHealthBigInteger -= bigInteger;

            DamageText damageText = Instantiate(_damageText, transform.position, UnityEngine.Quaternion.identity);
            Vector3 spawnPos = transform.position;
            bool isFull = true;
            for (int i = 0; i < _damageTextList.Count; i++)
            {
                if (_damageTextList[i] == null)
                {
                    _damageTextList[i] = damageText;
                    spawnPos += Vector3.up * i * 0.25f;
                    isFull = false;
                    break;
                }
            }
            if (isFull)
            {
                spawnPos += Vector3.up * _damageTextList.Count * 0.4f;
                _damageTextList.Add(damageText);
                if (_damageTextList.Count == 5) _damageTextList.Clear();
            }
            spawnPos += Vector3.right * Random.Range(-0.2f, 0.2f);
            damageText.Init(spawnPos, bigInteger.ParseNumber(), Color.yellow);

            if (CurrentHealthBigInteger < 0)
            {
                CurrentHealthBigInteger = 0;
                IsDead = true;
                _entity.OnDie();
            }
            OnHealthChangedEvent?.Invoke(prev, CurrentHealthBigInteger);
        }
    }
}
