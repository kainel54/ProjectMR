using UnityEngine;

namespace ProjectMR.Entities
{
    public class EntityMoveComponent : MonoBehaviour, IEntityComponent
    {
        private Vector2 _movement;
        [Header("AnimParams")]

        public Vector2 Velocity => _rbCompo.linearVelocity;
        public bool CanManualMove { get; set; } = true; //Ű����� ������ ����

        private Rigidbody2D _rbCompo;
        private Entity _entity;
        //private EntityRenderer _renderer;


        private Collider2D _collider;

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _rbCompo = entity.GetComponent<Rigidbody2D>();
            //_renderer = entity.GetCompo<EntityRenderer>();
            _collider = entity.GetComponent<Collider2D>();
        }


        public void AddForceToEntity(Vector2 force, ForceMode2D mode = ForceMode2D.Impulse)
        {
            _rbCompo.AddForce(force, mode);
        }

        public void StopImmediately()
        {
            _rbCompo.linearVelocity = Vector2.zero;
            _movement = Vector2.zero;
        }

        public void SetMovement(Vector2 movement) => _movement = movement;

        private void FixedUpdate()
        {
            MoveCharacter();
        }

        private void MoveCharacter()
        {
            if (CanManualMove)
                _rbCompo.linearVelocity = _movement;

            //_renderer.FlipController(_movement.x);
        }
    }
}
