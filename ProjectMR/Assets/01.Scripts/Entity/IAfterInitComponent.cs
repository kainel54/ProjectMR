using UnityEngine;

namespace ProjectMR.Entity
{
    public interface IAfterInitComponent
    {
        public void AfterInit();
        public void Dispose();
    }
}
