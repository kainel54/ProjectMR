using System;
using UnityEngine;

namespace ProjectMR.CustomAttributes
{
    /// <summary>
    /// Visible based on value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ToggleFieldAttribute : PropertyAttribute
    {
        public string PropertyName { get; private set; }
        public int EnumValue { get; private set; }
        public bool BoolValue { get; private set; }

        /// <summary>
        /// Shown when bool is true.
        /// </summary>
        /// <param name="boolProperty">BoolProperty variable name</param>
        /// <param name="boolValue">Bool value to show</param>
        public ToggleFieldAttribute(string boolProperty, bool boolValue = true)
        {
            PropertyName = boolProperty;
            BoolValue = boolValue;
        }
        /// <summary>
        /// Shown when enumProperty value equals enumValue.
        /// </summary>
        /// <param name="enumProperty">Enum variable name</param>
        /// <param name="enumValue">Enum value to show</param>
        public ToggleFieldAttribute(string enumProperty, int enumValue)
        {
            PropertyName = enumProperty;
            EnumValue = Convert.ToInt32(enumValue);
        }
    }
}