using ProjectMR.Animation;
using ProjectMR.Entities;
//using ProjectMR.Entities.Enemies;
using ProjectMR.Stat;
using UnityEngine;

namespace ProjectMR.FSM
{
    public class EnemyAttackState : StateBase
    {
        private readonly static int _AttackSpeedHash = Animator.StringToHash("AttackSpeed");

        //private Enemy _enemy;
        private EntityMoveComponent _entityMover;
        private EntityStat _entityStat;
        private StatElement _statElement;

        public EnemyAttackState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            //_enemy = entity as Enemy;
            _entityMover = entity.GetCompo<EntityMoveComponent>();
            _entityStat = entity.GetCompo<EntityStat>();
            _statElement = _entityStat.StatDictionary[StatName.AttackSpeed];
        }

        public override void Enter()
        {
            base.Enter();
            _entityMover.StopImmediately();
            _entityRenderer.PlayAnimation("Player_Attack");
            float attackSpeed = _statElement.Value * 60f / 37f;
            _entityRenderer.SetParam(_AttackSpeedHash, Mathf.Clamp(attackSpeed, 1f, 10000000f));
            //_enemy.CheckAttackTime();
        }

        public override void Update()
        {
            base.Update();

            if (HasTriggerCall(EAnimationEventType.Trigger))
            {
                //_enemy.Attack();
            }
            if (HasTriggerCall(EAnimationEventType.End))
            {
                _entityState.ChangeState(StateName.Idle);
            }
        }

        public override void Exit()
        {
            base.Exit();
            _entityRenderer.SetParam(_AttackSpeedHash, 1f);
        }
    }
}
