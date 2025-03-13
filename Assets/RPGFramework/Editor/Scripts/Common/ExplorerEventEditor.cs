using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LocationEvent))]
public class ExplorerEventEditor : Editor
{
    private LocationEvent locationEvent;

    private void OnEnable()
    {
        locationEvent = (LocationEvent)target;

        locationEvent.InnerEvent ??= CreateInstance<GraphEvent>();
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Label("Настройки");

        locationEvent.Interaction = (LocationEvent.InteractionType)EditorGUILayout.EnumPopup("Тип взаимодействия", locationEvent.Interaction);

        locationEvent.OnlyOne = EditorGUILayout.Toggle("Только раз?:", locationEvent.OnlyOne);
        locationEvent.Parallel = EditorGUILayout.Toggle("Паралельно?:", locationEvent.Parallel);

        if (locationEvent.InnerEvent != null && GUILayout.Button("Открыть редактор"))
            EventGraphWindow.Initialize(locationEvent.InnerEvent);

        if (GUILayout.Button("Пересоздать"))
            locationEvent.InnerEvent = CreateInstance<GraphEvent>();

        if (GUI.changed)
            EditorUtility.SetDirty(locationEvent);
    }
}