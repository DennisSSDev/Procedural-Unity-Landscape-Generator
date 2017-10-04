using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(Map))]
public class Button_To_GenMap : Editor {
    public override void OnInspectorGUI()
    {

        Map mapGen = (Map)target;
        if (DrawDefaultInspector())
        {
            if (mapGen.AutoUpdate)
                mapGen.GenerateMap();
        }
        if (GUILayout.Button("Generate Noise"))
        {
            mapGen.GenerateMap();
        }
    }
}
