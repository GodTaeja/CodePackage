#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(SoftSprite))]
public class SoftSpriteInspector : Editor
{
	SerializedProperty textureProp;
	SerializedProperty scaleProp;

    private Vector2 scaleVal;

	void OnEnable()
	{
		textureProp = serializedObject.FindProperty("Sprite");
		scaleProp = serializedObject.FindProperty("Scale");

		SetValues();
	}

	private void SetValues()
	{
		scaleVal = scaleProp.vector2Value;
	}

	private bool CheckForUpdate()
	{
		if (scaleProp.vector2Value != scaleVal)
		{
			SetValues();
			return true;
		}
		return false;
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		EditorGUILayout.PropertyField(textureProp, new GUIContent("Sprite"));
		EditorGUILayout.PropertyField(scaleProp, new GUIContent("Scale"));

		serializedObject.ApplyModifiedProperties();

		if (GUILayout.Button("Update") || CheckForUpdate())
		{
			foreach (var target in targets)
			{
				var softSprite = (SoftSprite)target;
				softSprite.ForceUpdate();
			}
		}
	}
}
#endif