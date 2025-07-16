using DG.Tweening;
using UnityEngine;

namespace ProjectMR.Core.Pool
{
    public class LifeTime : MonoBehaviour
    {
        protected float _lifeTime;
        public virtual void Init(float lifeTime, IPoolable poolItem)
        {
            _lifeTime = lifeTime;
            DOVirtual.DelayedCall(lifeTime, () =>
            {
                PoolManager.Instance.Push(poolItem);
            });
        }
    }

}
