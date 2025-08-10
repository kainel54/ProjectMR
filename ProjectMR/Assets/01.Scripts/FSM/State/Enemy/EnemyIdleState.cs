using ProjectMR.Animation;
using ProjectMR.Entities;
//using ProjectMR.Entities.Enemies;
using UnityEngine;

namespace ProjectMR.FSM
{
    public class EnemyIdleState : StateBase
    {
        //private Enemy _enemy;
        private EntityMoveComponent _entityMover;
        private float _duration;
        private float _startTime;

        public EnemyIdleState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            //_enemy = entity as Enemy;
            _entityMover = entity.GetCompo<EntityMoveComponent>();
            _startTime = Time.time;
            _duration = Random.Range(1f, 4f);
        }

        public override void Enter()
        {
            base.Enter();
            _entityMover.StopImmediately();
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
                    //    _entityRenderer.FlipController(Mathf.Sign(dir.x));
                    //}
                }
                else
                {
                    _entityState.ChangeState(StateName.Chase);
                }
            }
            else if (_startTime + _duration < Time.time)
            {
                _entityState.ChangeState(StateName.Patrol);
            }
        }
    }
}
