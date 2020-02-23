using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Grid),true)]
public class GridEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Grid g = GetSerializedPropertyObjectValue<Grid>(property);
        //Debug.Log(g.PosX);
        //EditorGUILayout.PropertyField(property.serializedObject.FindProperty("PosX"));
        //EditorGUILayout.PropertyField(property.serializedObject.FindProperty("PosY"));
        position.height = 20;
        g.PosX = EditorGUI.DelayedIntField(position,new GUIContent("X坐标"), g.PosX);
        position.y += 30;
        g.PosY = EditorGUI.DelayedIntField(position, new GUIContent("Y坐标"), g.PosY);
        if (GUILayout.Button("默认位置"))
        {
            g.PosX = 10;
            g.PosY = 10;
        }
        position.y += 30;
        g.Name = EditorGUI.DelayedTextField(position, new GUIContent("名字"), g.Name);
        position.y += 30;
        g.Property = (Property)EditorGUI.EnumPopup(position,new GUIContent("属性") ,g.Property);
        property.serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(property.serializedObject.targetObject);
    }
    public T GetSerializedPropertyObjectValue<T>(SerializedProperty property)
    {
        UnityEngine.Object targetObject = property.serializedObject.targetObject;
        System.Type targetObjectClassType = targetObject.GetType();
        Debug.Log(property.propertyPath);
        System.Reflection.FieldInfo field = targetObjectClassType.GetField(property.propertyPath);
        if (field != null)
        {
            T value = (T)field.GetValue(targetObject);
            return value;
        }

        return default(T);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 120;
    }
}
