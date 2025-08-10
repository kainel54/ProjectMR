using ProjectMR.Animation;
using ProjectMR.Entities;
using System;

namespace ProjectMR.FSM
{
    [Flags]
    public enum EAnimationEventType
    {
        Start = 1,
        End = 2,
        Trigger = 4,
    }

    [Serializable]
    public abstract class StateBase
    {
        protected Entity _entity;
        protected EntityState _entityState;
        
        protected AnimParamSO _animParam;
        private EAnimationEventType _isTriggerCall;

        protected EntityRenderer _entityRenderer;

        public StateBase(Entity entity, AnimParamSO animParam)
        {
            _entity = entity;
            _animParam = animParam;
            _entityRenderer = entity.GetCompo<EntityRenderer>(true);
            _entityState = entity.GetCompo<EntityState>();
        }

        public virtual void Enter()
        {
            _entityRenderer.SetParam(_animParam, true);
            _entityRenderer.OnAnimationEvent += HandleAnimationEvent;
            _isTriggerCall = 0;
        }

        protected virtual void HandleAnimationEvent(EAnimationEventType type)
        {
            AddTriggerCall(type);
        }
        public void AddTriggerCall(EAnimationEventType type) => _isTriggerCall |= type;
        public bool HasTriggerCall(EAnimationEventType type) => _isTriggerCall.HasFlag(type);
        public void RemoveTriggerCall(EAnimationEventType type) => _isTriggerCall &= ~type;

        public virtual void Update() { }

        public virtual void Exit()
        {
            _entityRenderer.OnAnimationEvent -= HandleAnimationEvent;
            _entityRenderer.SetParam(_animParam, false);
        }

        private void HandleAnimationEvent(Entities.EAnimationEventType type)
        {
            throw new NotImplementedException();
        }
    }
}
