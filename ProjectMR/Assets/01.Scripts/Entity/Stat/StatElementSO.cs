using ProjectMR.CustomAttributes;
using UnityEngine;

namespace ProjectMR.Stat
{
    [CreateAssetMenu(fileName = "StatElementSO", menuName = "SO/StatElementSO")]
    public class StatElementSO : ScriptableObject
    {
        public string statName;
        public string displayName;
        public bool isBigInteger;
        [ToggleField(nameof(isBigInteger), false)] public Vector2 minMaxValue;
        public Sprite statIcon;
    }
}
