using UnityEngine;
using UnityEditor;

[System.AttributeUsage(System.AttributeTargets.Field)]
public class ConditionalFieldAttribute : PropertyAttribute
{
    public string FieldToCheck;

    public ConditionalFieldAttribute(string fieldToCheck)
    {
        FieldToCheck = fieldToCheck;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ConditionalFieldAttribute))]
public class ConditionalFieldDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalFieldAttribute condAttr = (ConditionalFieldAttribute)attribute;
        SerializedProperty conditionField = property.serializedObject.FindProperty(condAttr.FieldToCheck);

        if (conditionField != null && conditionField.boolValue)
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalFieldAttribute condAttr = (ConditionalFieldAttribute)attribute;
        SerializedProperty conditionField = property.serializedObject.FindProperty(condAttr.FieldToCheck);

        if (conditionField != null && conditionField.boolValue)
        {
            return EditorGUI.GetPropertyHeight(property);
        }

        return 0;
    }
}
#endif
