using System;
using UnityEngine;

namespace ProjectMR.Entity
{
    public class EntityRenderer : MonoBehaviour,IEntityComponent
    {
        public float FacingDirection { get; private set; } = 1;

        [field: SerializeField] public bool IsFlip { get; private set; } = false;

        private Entity _entity;
        private Animator _animator;
        public virtual void Initialize(Entity entity)
        {
            _entity = entity;
            _animator = GetComponent<Animator>();
        }

        public event Action<EAnimationEventType> OnAnimationEvent;

        public void PlayAnimation(string name)
        {
            _animator?.Play(name);
        }


        public void SetParam(AnimParamSO param, bool value) => _animator.SetBool(param.hashValue, value);
        public void SetParam(AnimParamSO param, float value) => _animator.SetFloat(param.hashValue, value);
        public void SetParam(AnimParamSO param, int value) => _animator.SetInteger(param.hashValue, value);
        public void SetParam(AnimParamSO param) => _animator.SetTrigger(param.hashValue);

        public void SetParam(int hash, bool value) => _animator.SetBool(hash, value);
        public void SetParam(int hash, float value) => _animator.SetFloat(hash, value);
        public void SetParam(int hash, int value) => _animator.SetInteger(hash, value);
        public void SetParam(int hash) => _animator.SetTrigger(hash);

        #region FlipControl

        public void Flip()
        {
            FacingDirection *= -1;
            transform.Rotate(0, 180f, 0);
        }

        public void FlipController(float xMove)
        {
            if (Mathf.Abs(FacingDirection + xMove) < 0.5f ^ IsFlip)
                Flip();
        }

        #endregion

        private void AnimationEvent(EAnimationEventType eAnimationEventType)
        {
            OnAnimationEvent?.Invoke(eAnimationEventType);
        }
    }
}
