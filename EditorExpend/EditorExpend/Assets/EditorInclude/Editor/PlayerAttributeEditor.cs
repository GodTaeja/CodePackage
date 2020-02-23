using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerAttribute))]
public class PlayerAttributeEditor : Editor {

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        PlayerAttribute player = (PlayerAttribute)target;

        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Name"));
        if (GUILayout.Button("Print"))
        {
            Debug.Log(serializedObject.targetObject.name);
        }
        EditorGUILayout.DelayedIntField(serializedObject.FindProperty("Id"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("grid"));
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }
}
