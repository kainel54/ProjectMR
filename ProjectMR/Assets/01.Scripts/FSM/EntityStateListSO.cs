using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMR.FSM
{
    public static class StateName
    {
        public readonly static string Idle = "Idle";
        public readonly static string Patrol = "Patrol";
        public readonly static string Chase = "Chase";
        public readonly static string Attack = "Attack";
        public readonly static string Die = "Die";
    }

    [CreateAssetMenu(fileName = "EntityStateListSO", menuName = "SO/FSM/EntityStateList")]
    public class EntityStateListSO : ScriptableObject
    {
        public List<StateSO> states;
    }
}
