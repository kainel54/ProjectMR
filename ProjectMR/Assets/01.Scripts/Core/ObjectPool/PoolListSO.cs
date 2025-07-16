using System.Collections.Generic;
using UnityEngine;

namespace ProjectMR.Core.Pool
{
    [CreateAssetMenu(menuName = "SO/Pool/PoolList")]
    public class PoolListSO : ScriptableObject
    {
        [SerializeField] private List<PoolingItemSO> _list;

        public List<PoolingItemSO> GetList() => _list;
    }
}

