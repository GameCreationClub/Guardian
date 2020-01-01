using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Object))]
public class ObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Object o = (Object)target;

        EditorGUILayout.IntField("X", o.X);
        EditorGUILayout.IntField("Y", o.Y);
    }
}
