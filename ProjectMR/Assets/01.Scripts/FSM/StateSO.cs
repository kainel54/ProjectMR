using ProjectMR.Animation;
using UnityEngine;

namespace ProjectMR.FSM
{
    [CreateAssetMenu(fileName = "StateSO", menuName = "SO/FSM/StateSO")]
    public class StateSO : ScriptableObject
    {
        public string StateTarget;
        public string StateName;
        public AnimParamSO animParamSO;
    }
}
