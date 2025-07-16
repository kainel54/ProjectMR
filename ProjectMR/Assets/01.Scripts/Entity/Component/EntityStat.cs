using ProjectMR.Stat;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMR.Entity
{
    public class EntityStat : MonoBehaviour,IEntityComponent
    {
        public List<StatElement> _overrideStatElementList;
        public StatBaseSO _baseStatSO;
        public StatDictionary StatDictionary { get; private set; }

        public void Initialize(Entity entity)
        {
            StatDictionary = new StatDictionary(_overrideStatElementList, _baseStatSO);
        }
    }
}
