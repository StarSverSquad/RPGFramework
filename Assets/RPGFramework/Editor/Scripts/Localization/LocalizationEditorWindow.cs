using RPGF.Core.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class LocalizationEditorWindow : EditorWindow
{
    private LocalizationSheet sheet;

    private string tagBuffer;
    private Vector2 scroll;

    private string searchText;

    private Dictionary<string, bool> localeFoldouts = new();

    public static LocalizationEditorWindow Create(LocalizationSheet sheet)
    {
        LocalizationEditorWindow win = GetWindow<LocalizationEditorWindow>(true, "Редактор локализации", true);

        win.sheet = sheet;

        return win;
    }

    private void OnGUI()
    {
        searchText = EditorGUILayout.TextField("Поиск:", searchText);

        GUILayout.BeginHorizontal();
        tagBuffer = EditorGUILayout.TextField("Тег:", tagBuffer);

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

        GUILayout.BeginVertical();
        scroll = EditorGUILayout.BeginScrollView(scroll);

        var locales = sheet.locales.data.Where(locale => Regex.IsMatch(locale.Key, @$"\w*{searchText}\w*")).ToList();
        for (int i = 0; i < locales.Count; i++)
        {
            var locale = locales[i];

            if (!localeFoldouts.Keys.Any(key => key == locale.Key))
                localeFoldouts.Add(locale.Key, false);

            LocaleElement(locale.Key, locale.Value);
        }


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