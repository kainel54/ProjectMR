using ProjectMR.Animation;
using ProjectMR.Entities;
//using ProjectMR.Entities.Enemies;
using ProjectMR.Stat;
using UnityEngine;

namespace ProjectMR.FSM
{
    public class EnemyPatrolState : StateBase
    {
        //private Enemy _enemy;
        private EntityMoveComponent _entityMover;
        private StatElement _speedStat;
        private float _duration;
        private float _startTime;

        public EnemyPatrolState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            //_enemy = entity as Enemy;
            _entityMover = entity.GetCompo<EntityMoveComponent>();
            _speedStat = entity.GetCompo<EntityStat>().StatDictionary[StatName.MoveSpeed];
        }

        public override void Enter()
        {
            base.Enter();
            Vector2 random = Random.insideUnitCircle;
            _entityMover.SetMovement(random * _speedStat.Value * 0.7f);
            _entityRenderer.FlipController(Mathf.Sign(random.x));
            _startTime = Time.time;
            _duration = Random.Range(1f, 3f);
        }

        public override void Update()
        {
            base.Update();
            

            if(_entity.IsTargetInRange(_entity.TargetDetectRange, out Collider2D collider))
            {
                Vector2 dir = collider.transform.position - _entity.transform.position;
                if (dir.magnitude < _entity.AttackRange)
                {
                    _entityState.ChangeState(StateName.Idle);
                }
                else
                {
                    _entityState.ChangeState(StateName.Chase);
                }
            }
            else if (_startTime + _duration < Time.time)
            {
                _entityState.ChangeState(StateName.Idle);
            }
        }
    }
}
