using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GraphEvent))]
public class GraphEventScriptableEditor : Editor
{
    private GraphEvent gEvent;

    private void OnEnable()
    {
        gEvent = (GraphEvent)target;
    }

    private void OnDisable()
    {
        
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Open editor"))
            EventGraphWindow.Initialize(gEvent);
    }
}
