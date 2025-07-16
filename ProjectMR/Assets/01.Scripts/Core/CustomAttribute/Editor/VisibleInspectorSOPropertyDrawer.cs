using ProjectMR.CustomAttributes;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(VisibleInspectorSOAttribute))]
public class VisibleInspectorSOPropertyDrawer : PropertyDrawer
{
    private Dictionary<string, bool> _isExpandedDict = new Dictionary<string, bool>();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        string propertyPath = property.propertyPath;
        bool isExpanded = _isExpandedDict.ContainsKey(propertyPath) ? _isExpandedDict[propertyPath] : false;

        Rect foldoutRect = new Rect(position.x - 1, position.y + (EditorGUI.GetPropertyHeight(property, null, true)) / 2 - 10, Mathf.Max(120, (EditorGUIUtility.currentViewWidth) * 0.45f - 40), 20);
        _isExpandedDict[propertyPath] = EditorGUI.Foldout(foldoutRect, isExpanded, label, true);

        GUIContent emptyLabel = label;
        emptyLabel.text = " ";
        EditorGUI.PropertyField(position, property, emptyLabel, true);

        if (isExpanded && property.objectReferenceValue != null)
        {
            EditorGUI.indentLevel++;
            SerializedObject so = new SerializedObject(property.objectReferenceValue);
            so.Update();

            float soRefSize = EditorGUI.GetPropertyHeight(property, null, false);
            position.y += soRefSize + EditorGUIUtility.standardVerticalSpacing / 2 + 7;
            position.height -= soRefSize + EditorGUIUtility.standardVerticalSpacing / 2 + 9;

            Rect backgroundOutRect = new Rect(position.x, position.y - 5, position.width, position.height);
            EditorGUI.DrawRect(backgroundOutRect, new Color(0.15f, 0.15f, 0.15f));
            float border = 1f;
            Rect backgroundInRect = new Rect(position.x + border, position.y - 5 + border, position.width - border * 2, position.height - border * 2);
            EditorGUI.DrawRect(backgroundInRect, new Color(0.2549f, 0.2549f, 0.2549f));

            position.x += 5f;
            position.width -= 15f;

            SerializedProperty soProperty = so.GetIterator();
            soProperty.NextVisible(true);
            while (soProperty.NextVisible(false))
            {
                position.height = EditorGUI.GetPropertyHeight(soProperty, null, true);
                EditorGUI.PropertyField(position, soProperty, true);
                position.y += position.height + 2;
            }

            so.ApplyModifiedProperties();
            EditorGUI.indentLevel--;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // �⺻ �Ӽ� ���� ���
        float height = EditorGUI.GetPropertyHeight(property, label, true);

        string propertyPath = property.propertyPath;
        bool isExpanded = _isExpandedDict.ContainsKey(propertyPath) ? _isExpandedDict[propertyPath] : false;

        if (isExpanded && property.objectReferenceValue != null)
        {
            // �Ӽ��� SO�� ���� ��� �� SO�� �Ӽ����� �߰��� ���
            SerializedObject so = new SerializedObject(property.objectReferenceValue);
            so.Update();

            SerializedProperty soProperty = so.GetIterator();
            soProperty.NextVisible(true);
            do
            {
                height += EditorGUI.GetPropertyHeight(soProperty, null, true) + EditorGUIUtility.standardVerticalSpacing;
            }
            while (soProperty.NextVisible(false));


            height -= EditorGUIUtility.standardVerticalSpacing;
        }

        return height;
    }
}
