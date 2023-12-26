using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RPGConsumed))]
public class RPGConsumedEditor : Editor
{
    private RPGConsumed consumed;

    private EffectEditorService effectEditorService;

    private int selected = 0;

    private void OnEnable()
    {
        consumed = (RPGConsumed)target;

        effectEditorService = new EffectEditorService();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Type[] types = consumed.GetType().Assembly.GetTypes()
            .Where(i => i.BaseType != null && i.BaseType.Name == "EffectBase").ToArray();

        List<string> names = new List<string>();
        foreach (Type t in types)
        {
            EffectBase effect = consumed.GetType().Assembly.CreateInstance(t.Name) as EffectBase;

            names.Add(effect.GetName());
        }

        EditorGUILayout.BeginVertical(GUI.skin.box);

        GUILayout.Label("Добавление эффекта");

        selected = EditorGUILayout.Popup(selected, names.ToArray());

        if (GUILayout.Button("Добавить"))
            consumed.Effects.Add(consumed.GetType().Assembly.CreateInstance(types[selected].Name) as EffectBase);


        for (int i = 0; i < consumed.Effects.Count; i++)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            GUILayout.Label(consumed.Effects[i].GetName());

            effectEditorService.BuildGUI(consumed.Effects[i]);

            if (GUILayout.Button("Удалить", GUILayout.Width(100), GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
                consumed.Effects.RemoveAt(i);

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();

        if (GUI.changed)
            EditorUtility.SetDirty(consumed);
    }
}