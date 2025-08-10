using ProjectMR.Animation;
using ProjectMR.Entities;
using ProjectMR.Stat;
using UnityEngine;

namespace ProjectMR.FSM
{
    public class EnemyChaseState : StateBase
    {
        //private Enemy _enemy;
        private EntityMoveComponent _entityMover;
        private StatElement _speedStat;

        public EnemyChaseState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            //_enemy = entity as Enemy;
            _entityMover = entity.GetCompo<EntityMoveComponent>();
            _speedStat = entity.GetCompo<EntityStat>().StatDictionary[StatName.MoveSpeed];
        }

        public override void Update()
        {
            base.Update();


            if (_entity.IsTargetInRange(_entity.TargetDetectRange, out Collider2D collider))
            {
                Vector2 dir = collider.transform.position - _entity.transform.position;
                if (dir.magnitude < _entity.AttackRange)
                {
                    //if (_enemy.IsCanAttack())
                    //{
                    //    _entityState.ChangeState(StateName.Attack);
                    //}
                    //else
                    //{
                    //    _entityState.ChangeState(StateName.Idle);
                    //}
                }
                else
                {
                    _entityMover.SetMovement(dir.normalized * _speedStat.Value);
                    _entityRenderer.FlipController(Mathf.Sign(dir.x));
                }
            }
            else
            {
                _entityState.ChangeState(StateName.Idle);
            }

        }
    }
}
