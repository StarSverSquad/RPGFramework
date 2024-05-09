using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LocalizationEditorWindow : EditorWindow
{
    private LocalizationSheet sheet;

    private string tagBuffer;
    private Vector2 scroll;

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
            LocaleElement(sheet.locales.data[i].Key, sheet.locales.data[i].Value);

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

            sheet.locales.Add(tagBuffer, "");
        }

        
        GUILayout.EndHorizontal();

        GUILayout.EndScrollView();
        GUILayout.EndVertical();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(sheet);
        }
    }

    private void LocaleElement(string tag, string value)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.SelectableLabel($"<%{tag}>");
        EditorGUILayout.Space(3);
        sheet.locales[tag] = EditorGUILayout.TextArea(value);

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