using ProjectMR.Animation;
using ProjectMR.Core;
using ProjectMR.Entities;
//using ProjectMR.Entities.Enemies;
//using ProjectMR.Entities.Players;
using ProjectMR.Stat;
using UnityEngine;

namespace ProjectMR.FSM
{
    public class PlayerAttackState : StateBase
    {
        private readonly static int _AttackSpeedHash = Animator.StringToHash("AttackSpeed");

        //private Player _player;
        private EntityMoveComponent _entityMover;
        private EntityStat _entityStat;
        private StatElement _statElement;

        //private Enemy _targetEnemy;

        public PlayerAttackState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            //_player = entity as Player;
            _entityMover = entity.GetCompo<EntityMoveComponent>();
            _entityStat = entity.GetCompo<EntityStat>();
            _statElement = _entityStat.StatDictionary[StatName.AttackSpeed];
        }

        public override void Enter()
        {
            base.Enter();
            _entityMover.StopImmediately();
            float attackSpeed = _statElement.Value * 37f / 60f;
            _entityRenderer.SetParam(_AttackSpeedHash, Mathf.Clamp(attackSpeed, 1f, 10000000f));
            //_player.CheckAttackTime();

            if (_entity.IsTargetInRange(1.5f, out Collider2D collider))
            {
                //_targetEnemy = collider.GetComponent<Enemy>();
            }
        }

        public override void Update()
        {
            base.Update();

            //if (_targetEnemy.gameObject.activeSelf)
            //{
            //    Vector3 dir = _targetEnemy.transform.position - _player.transform.position;
            //    _entityRenderer.FlipController(Mathf.Sign(dir.x));
            //}

            //if (HasTriggerCall(EAnimationEventType.Trigger))
            //{
            //    RemoveTriggerCall(EAnimationEventType.Trigger);
            //    _player.Attack(_targetEnemy);
            //}
            //if (HasTriggerCall(EAnimationEventType.End))
            //{
            //    _entityState.ChangeState(StateName.Idle);
            //}
        }

        public override void Exit()
        {
            base.Exit();
            _entityRenderer.SetParam(_AttackSpeedHash, 1f);
        }
    }
}
