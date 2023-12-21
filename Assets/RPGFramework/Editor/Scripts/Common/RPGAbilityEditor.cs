using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RPGAbility))]
public class RPGAbilityEditor : Editor
{
    private RPGAbility ability;

    private EffectEditorService effectEditorService;

    private int selected = 0;

    private void OnEnable()
    {
        ability = (RPGAbility)target;

        effectEditorService = new EffectEditorService();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Type[] types = ability.GetType().Assembly.GetTypes()
            .Where(i => i.BaseType != null && i.BaseType.Name == "EffectBase").ToArray();

        List<string> names = new List<string>();
        foreach (Type t in types)
        {
            EffectBase effect = ability.GetType().Assembly.CreateInstance(t.Name) as EffectBase;

            names.Add(effect.GetName());
        }

        EditorGUILayout.BeginVertical(GUI.skin.box);

        GUILayout.Label("Добовление эффекта");

        selected = EditorGUILayout.Popup(selected, names.ToArray());

        if (GUILayout.Button("Добавить"))
            ability.Effects.Add(ability.GetType().Assembly.CreateInstance(types[selected].Name) as EffectBase);


        for (int i = 0; i < ability.Effects.Count; i++)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            GUILayout.Label(ability.Effects[i].GetName());

            effectEditorService.BuildGUI(ability.Effects[i]);

            if (GUILayout.Button("Удалить", GUILayout.Width(100), GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
                ability.Effects.RemoveAt(i);

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();

        if (GUI.changed)
            EditorUtility.SetDirty(ability);
    }
}