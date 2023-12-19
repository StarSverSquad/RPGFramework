using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ExplorerEvent))]
public class ExplorerEventEditor : Editor
{
    private ExplorerEvent oe;

    private void OnEnable()
    {
        oe = (ExplorerEvent)target;

        oe.Event ??= CreateInstance<GraphEvent>();

        if (string.IsNullOrEmpty(oe.GUID))
        {
            oe.GUID = Guid.NewGuid().ToString();

            EditorUtility.SetDirty(oe);
        }
    }

    public override void OnInspectorGUI()
    {
        oe.Interaction = (ExplorerEvent.InteractionType)EditorGUILayout.EnumPopup("Тип взаимодействия", oe.Interaction);

        oe.DynamicOrderChange = EditorGUILayout.Toggle("Динам. изм. Z координаты?", oe.DynamicOrderChange);

        oe.OnlyOne = EditorGUILayout.Toggle("Только раз?:", oe.OnlyOne);
        oe.CacheResult = EditorGUILayout.Toggle("Кэшировать результат?", oe.CacheResult);
        oe.Parallel = EditorGUILayout.Toggle("Паралельно?:", oe.Parallel);

        if (oe.Event != null && GUILayout.Button("Открыть редактор"))
            EventGraphWindow.Initialize(oe.Event);

        if (GUI.changed)
            EditorUtility.SetDirty(oe);
    }
}