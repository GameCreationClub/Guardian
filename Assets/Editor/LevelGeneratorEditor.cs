using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LevelGenerator levelGenerator = (LevelGenerator)target;

        if (GUILayout.Button("Generate Level"))
        {
            levelGenerator.GenerateLevel();
        }

        if (GUILayout.Button("Clear Level"))
        {
            levelGenerator.ClearLevel();
        }
    }
}
