using System;
using UnityEngine;

namespace ProjectMR.CustomAttributes
{
    /// <summary>
    /// Disables changing in the inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class UncorrectableAttribute : PropertyAttribute
    {
        public UncorrectableAttribute() { }
    }
}
