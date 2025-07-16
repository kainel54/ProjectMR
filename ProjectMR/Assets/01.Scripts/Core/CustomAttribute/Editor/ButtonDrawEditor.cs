using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ProjectMR.CustomAttributes
{
    public struct MethodButtonInfo
    {
        public ButtonAttribute attribute;
        public MethodInfo methodInfo;
    }

    [CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
    public class ButtonDrawEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            foreach (var targetObject in targets)
            {
                // GetMethods
                MethodInfo[] methods = targetObject.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                Dictionary<string, List<MethodButtonInfo>> methodDict = new Dictionary<string, List<MethodButtonInfo>>();
                foreach (MethodInfo method in methods)
                {
                    ButtonAttribute buttonAttr = (ButtonAttribute)Attribute.GetCustomAttribute(method, typeof(ButtonAttribute));

                    if (buttonAttr != null)
                    {
                        string tag = buttonAttr.Tag ?? method.GetHashCode().ToString();

                        MethodButtonInfo methodButtonInfo = new MethodButtonInfo();
                        methodButtonInfo.attribute = buttonAttr;
                        methodButtonInfo.methodInfo = method;
                        if (methodDict.TryGetValue(tag, out List<MethodButtonInfo> buttonInfoList))
                            buttonInfoList.Add(methodButtonInfo);
                        else
                            methodDict[tag] = new List<MethodButtonInfo> { methodButtonInfo };
                    }
                }

                foreach (string tag in methodDict.Keys)
                {
                    GUILayout.BeginHorizontal();
                    foreach (MethodButtonInfo buttonInfo in methodDict[tag])
                    {
                        if (GUILayout.Button(buttonInfo.attribute.Label, GUILayout.Height(buttonInfo.attribute.Hight)))
                            buttonInfo.methodInfo.Invoke(targetObject, null);
                    }
                    GUILayout.EndHorizontal();
                }
            }
        }
    }
}
