using System;
using UnityEngine;

namespace ProjectMR.CustomAttributes
{
    /// <summary>
    /// Create a button in the inspector that executes a function.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : PropertyAttribute
    {
        public string Label { get; private set; }
        public int Hight { get; private set; }
        public string Tag { get; private set; }

        /// <param name="buttonLabel">Button name</param>
        /// <param name="height">Button height</param>
        /// <param name="tag">Button group tag</param>
        public ButtonAttribute(string buttonLabel = "Button", int hight = 20, string tag = null)
        {
            Label = buttonLabel;
            Hight = hight;
            Tag = tag;
        }
    }
}
