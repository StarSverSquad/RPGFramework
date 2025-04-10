using RPGF.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LocalizationEditorWindow : EditorWindow
{
    private LocalizationSheet sheet;

    private string tagBuffer;
    private Vector2 scroll;

    private Dictionary<string, bool> localeFoldouts = new Dictionary<string, bool>();

    public static LocalizationEditorWindow Create(LocalizationSheet sheet)
    {
        LocalizationEditorWindow win = GetWindow<LocalizationEditorWindow>(true, "Редактор локализации", true);

        win.sheet = sheet;

        return win;
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        scroll = EditorGUILayout.BeginScrollView(scroll);

        for (int i = 0; i < sheet.locales.Count(); i++)
        {
            if (!localeFoldouts.Keys.Any(key => key == sheet.locales.data[i].Key))
                localeFoldouts.Add(sheet.locales.data[i].Key, false);

            LocaleElement(sheet.locales.data[i].Key, sheet.locales.data[i].Value);
        }

        GUILayout.BeginHorizontal();

        tagBuffer = EditorGUILayout.TextField(tagBuffer);

        if (GUILayout.Button("Создать"))
        {
            if (string.IsNullOrEmpty(tagBuffer))
            {
                Debug.LogWarning("Тег не может быть пустым!");
                return;
            }
            else if (sheet.locales.HaveKey(tagBuffer))
            {
                Debug.LogWarning($"Локаль с тегом <{tagBuffer}> уже существует!");
                return;
            }

            sheet.locales.Add(tagBuffer, new Locale());
        }
        
        GUILayout.EndHorizontal();

        GUILayout.EndScrollView();
        GUILayout.EndVertical();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(sheet);
            Undo.RecordObject(this, "LEW changed");
        }

        Event e = Event.current;
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Z && e.control)
        {
            Undo.PerformUndo(); // Выполняем отмену
            e.Use(); // Помечаем событие как обработанное
        }
    }

    private void LocaleElement(string tag, Locale value)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.SelectableLabel($"<%{tag}>");
        EditorGUILayout.Space(3);

        var localeFields = value.GetType().GetFields()
            .Where(field => field.CustomAttributes
                .Any(attr => attr.AttributeType == typeof(LocalizationField)));

        localeFoldouts[tag] = EditorGUILayout.BeginFoldoutHeaderGroup(localeFoldouts[tag], "Языки");
        if (localeFoldouts[tag])
        {
            foreach (var field in localeFields)
            {
                GUILayout.Label(field.Name);
                field.SetValue(value, EditorGUILayout.TextArea(field.GetValue(value) as string));
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Удалить"))
        {
            if (EditorUtility.DisplayDialog("Warning", $"Вы действить хотите удалить елемент {tag}?", "Да", "Нет"))
            {
                sheet.locales.Remove(tag);

                EditorUtility.SetDirty(sheet);

                Repaint();
            }
        }

        GUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }
}