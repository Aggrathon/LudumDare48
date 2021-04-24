using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MazeManager.PrefabProbability))]
public class PrefabProbabilityDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, null, property);
        Rect probPos = new Rect(position.x, position.y, position.width / 3, position.height);
        Rect prefabPos = new Rect(probPos.xMax + 5, position.y, position.width * 2 / 3 - 5, position.height);
        EditorGUI.PropertyField(probPos, property.FindPropertyRelative("prob"), GUIContent.none);
        EditorGUI.PropertyField(prefabPos, property.FindPropertyRelative("prefab"), GUIContent.none);
        EditorGUI.EndProperty();
    }
}
