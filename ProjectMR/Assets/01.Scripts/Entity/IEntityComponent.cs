using UnityEngine;

namespace ProjectMR.Entities
{
    public interface IEntityComponent
    {
        public void Initialize(Entity entity);
    }
}
