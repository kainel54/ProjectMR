using UnityEngine;

namespace ProjectMR.Animation
{
    [CreateAssetMenu(fileName = "AnimParmSO", menuName = "Scriptable Objects/AnimParmSO")]
    public class AnimParamSO : ScriptableObject
    {
        public enum ParamType
        {
            Boolean, Float, Integer, Trigger
        }

        public string paramName;
        public ParamType paramType;
        public int hashValue;

        private void OnValidate()
        {
            hashValue = Animator.StringToHash(paramName);
        }
    }
}
