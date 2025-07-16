using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProjectMR.CustomAttributes
{
    [CustomPropertyDrawer(typeof(ToggleFieldAttribute))]
    public class ToggleFieldPropertyDrawer : PropertyDrawer
    {
        private Dictionary<string, bool> _isExpandedDict = new Dictionary<string, bool>();

        private const float _ChiledInterval = 15f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float startXPos = position.x;
            // ToggleFieldAttribute�� ��������
            ToggleFieldAttribute toggleField = (ToggleFieldAttribute)attribute;

            // ���� ��ü���� �ش� �Ӽ��� ã��
            SerializedProperty toggleProperty = property.serializedObject.FindProperty(toggleField.PropertyName);

            bool isEnabled = false;
            if (toggleProperty != null)
            {
                if (toggleProperty.propertyType == SerializedPropertyType.Boolean)
                {
                    isEnabled = toggleProperty.boolValue == toggleField.BoolValue;
                }
                else if (toggleProperty.propertyType == SerializedPropertyType.Enum)
                {
                    isEnabled = toggleProperty.enumValueIndex == toggleField.EnumValue;
                }
            }

            // ��� ���� ���� �Ӽ��� Ȱ��ȭ/��Ȱ��ȭ
            if (isEnabled)
            {
                EditorGUI.indentLevel++;

                // ���� ��
                float lineHight = EditorGUI.GetPropertyHeight(property, null, false) + EditorGUIUtility.standardVerticalSpacing;
                float lineWidth = 8;
                float lineXPos = position.x + _ChiledInterval * (EditorGUI.indentLevel - 1.5f);

                Rect verticalLine = new Rect(lineXPos, position.y - lineHight / 2 - 0.5f, 1.5f, lineHight + 1);
                EditorGUI.DrawRect(verticalLine, Color.gray);
                Rect horizontalLine = new Rect(lineXPos, position.y + lineHight / 2, lineWidth, 1);
                EditorGUI.DrawRect(horizontalLine, Color.gray);

                if (property.propertyType == SerializedPropertyType.Generic) // ����ü ó��
                {
                    string propertyPath = property.propertyPath;
                    bool isExpanded = _isExpandedDict.ContainsKey(propertyPath) ? _isExpandedDict[propertyPath] : false;

                    position.height = EditorGUI.GetPropertyHeight(property, null, false);
                    _isExpandedDict[propertyPath] = EditorGUI.Foldout(position, isExpanded, label, true);

                    if (isExpanded)
                    {
                        // ����ü�� �ʵ� ������
                        EditorGUI.indentLevel++;
                        position.y += position.height + EditorGUIUtility.standardVerticalSpacing;

                        SerializedProperty childProperty = property.Copy();
                        SerializedProperty endProperty = property.GetEndProperty();

                        childProperty.NextVisible(true); // ù ��° �ڽ� �ʵ�� �̵�
                        while (!SerializedProperty.EqualContents(childProperty, endProperty))
                        {
                            position.height = EditorGUI.GetPropertyHeight(childProperty, null, true);

                            Rect chiledVerticalLine = new Rect(lineXPos, position.y - lineHight / 2, 1.5f, position.height + EditorGUIUtility.standardVerticalSpacing);
                            EditorGUI.DrawRect(chiledVerticalLine, Color.gray);
                            Rect chiledHorizontalLine = new Rect(lineXPos, position.y + lineHight / 2, lineWidth + _ChiledInterval, 1);  
                            EditorGUI.DrawRect(chiledHorizontalLine, Color.gray);

                            EditorGUI.PropertyField(position, childProperty, true);
                            position.y += position.height + EditorGUIUtility.standardVerticalSpacing;

                            childProperty.NextVisible(false);
                        }
                        EditorGUI.indentLevel--;
                    }
                }
                else
                {
                    // �Ϲ� �ʵ� ó��
                    EditorGUI.PropertyField(position, property, label, true);
                }
                EditorGUI.indentLevel--;
            }
        }

        public float AddHorizontalInterval(ref Rect rect, float interval)
        {
            float prevLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth -= interval;
            rect.x += interval;
            rect.width -= interval;
            return prevLabelWidth;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ToggleFieldAttribute toggleField = (ToggleFieldAttribute)attribute;
            SerializedProperty toggleProperty = property.serializedObject.FindProperty(toggleField.PropertyName);

            bool isEnabled = false;
            if (toggleProperty != null)
            {
                if (toggleProperty.propertyType == SerializedPropertyType.Boolean)
                {
                    isEnabled = toggleProperty.boolValue == toggleField.BoolValue;
                }
                else if (toggleProperty.propertyType == SerializedPropertyType.Enum)
                {
                    isEnabled = toggleProperty.enumValueIndex == toggleField.EnumValue;
                }
            }

            if (isEnabled)
            {
                string propertyPath = property.propertyPath;
                bool isExpanded = _isExpandedDict.ContainsKey(propertyPath) ? _isExpandedDict[propertyPath] : false;

                if (property.propertyType == SerializedPropertyType.Generic && isExpanded)
                {
                    float height = EditorGUI.GetPropertyHeight(property, label, false);
                    if (property.hasVisibleChildren)
                    {
                        SerializedProperty tempProperty = property.Copy(); // ���� ��ġ ����

                        if (property.NextVisible(true)) // ���� �Ӽ��� �����ϴ��� Ȯ��
                        {
                            do
                            {
                                height += EditorGUI.GetPropertyHeight(property, null, true) + EditorGUIUtility.standardVerticalSpacing;
                            } while (property.NextVisible(false) && !SerializedProperty.EqualContents(property, tempProperty.GetEndProperty()));
                        }

                        property = tempProperty; // ���� ��ġ�� ����
                    }
                    height += EditorGUIUtility.standardVerticalSpacing;
                    return height;
                }
                else
                    return EditorGUIUtility.singleLineHeight;
            }
            else
                return -EditorGUIUtility.standardVerticalSpacing;
        }
    }
}
