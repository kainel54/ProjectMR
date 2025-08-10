using UnityEngine;

namespace ProjectMR.Entities
{
    public interface IAfterInitComponent
    {
        public void AfterInit();
        public void Dispose();
    }
}
