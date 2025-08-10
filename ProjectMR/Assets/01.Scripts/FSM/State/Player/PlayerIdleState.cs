using ProjectMR.Animation;
using ProjectMR.Core;
using ProjectMR.Entities;
using ProjectMR.Stat;
using UnityEngine;

namespace ProjectMR.FSM
{
    public class PlayerIdleState : StateBase
    {
        //private Player _player;
        private EntityMoveComponent _entityMover;
        //private PlayerRenderer _playerRenderer;
        private StatElement _speedStat;

        public PlayerIdleState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            //_player = entity as Player;
            _entityMover = entity.GetCompo<EntityMoveComponent>();
            _speedStat = entity.GetCompo<EntityStat>().StatDictionary[StatName.MoveSpeed];
        }

        public override void Enter()
        {
            base.Enter();
            _entityMover.StopImmediately();
        }

        public override void Update()
        {
            base.Update();

            //if (PlayerManager.Instance.IsAutoMode)
            //{
            //    _entityState.ChangeState(StateName.Chase);
            //}
            //else
            //{
            //    if (_entity.IsTargetInRange(_entity.TargetDetectRange, out Collider2D collider))
            //    {
            //        Vector2 dir = collider.transform.position - _entity.transform.position;
            //        if (dir.magnitude < _entity.AttackRange)
            //        {
            //            if (_player.IsCanAttack())
            //            {
            //                _entityState.ChangeState(StateName.Attack);
            //            }
            //            else
            //            {
            //                _entityRenderer.FlipController(Mathf.Sign(dir.x));
            //            }
            //        }
            //        else
            //        {
            //            _entityState.ChangeState(StateName.Chase);
            //        }
            //    }
            //}
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}