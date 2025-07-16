using System;
using UnityEngine;

namespace ProjectMR.CustomAttributes
{
    /// <summary>
    /// Enables SO inspector view.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class VisibleInspectorSOAttribute : PropertyAttribute
    {
        public VisibleInspectorSOAttribute() { }
    }
}
