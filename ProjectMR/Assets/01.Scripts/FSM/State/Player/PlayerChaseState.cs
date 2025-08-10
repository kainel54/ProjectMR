using ProjectMR.Animation;
using ProjectMR.Core;
using ProjectMR.Entities;
//using ProjectMR.Entities.Players;
using ProjectMR.Stat;
using UnityEngine;

namespace ProjectMR.FSM
{
    public class PlayerChaseState : StateBase
    {
        //private Player _player;
        private EntityMoveComponent _entityMover;
        private EntityMoveComponent _playerRenderer;
        private StatElement _speedStat;

        public PlayerChaseState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            //_player = entity as Player;
            _entityMover = entity.GetCompo<EntityMoveComponent>();
            _speedStat = entity.GetCompo<EntityStat>().StatDictionary[StatName.MoveSpeed];
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();

            //if (PlayerManager.Instance.IsAutoMode)
            //{
            //    Vector2 playerMoveInput = PlayerManager.Instance.PlayerMoveInput;
            //    _entityMover.SetMovement(playerMoveInput.normalized * _speedStat.Value);
            //    _entityRenderer.FlipController(Mathf.Sign(playerMoveInput.x));
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
            //                _entityState.ChangeState(StateName.Idle);
            //            }
            //        }
            //        else
            //        {
            //            _entityMover.SetMovement(dir.normalized * _speedStat.Value);
            //            _entityRenderer.FlipController(Mathf.Sign(dir.x));
            //        }
            //    }
            //    else
            //    {
            //        _entityState.ChangeState(StateName.Idle);
            //    }
            //}
        }
    }
}
