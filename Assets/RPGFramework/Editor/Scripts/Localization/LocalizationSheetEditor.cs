using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LocalizationSheet))]
public class LocalizationSheetEditor : Editor
{
    private LocalizationSheet sheet;

    private void OnEnable()
    {
        sheet = target as LocalizationSheet;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Открыть редактор"))
        {
            LocalizationEditorWindow.Create(sheet).Show();
        }
    }
}