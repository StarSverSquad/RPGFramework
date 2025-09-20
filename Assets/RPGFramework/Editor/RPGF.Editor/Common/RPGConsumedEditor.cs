using RPGF.Core.RPGEffect;
using RPGF.Editor.Core;
using RPGF.Editor.Core.Services;
using RPGF.RPG;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RPGF.Editor
{
    [CustomEditor(typeof(RPGConsumed))]
    public class RPGConsumedEditor : RPGFrameworkEditor<RPGConsumed>
    {
        private EffectEditorService effectEditorService;
        private int selected = 0;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Type[] types = Target.GetType().Assembly.GetTypes()
                .Where(i => i.BaseType != null && i.BaseType.Name == "EffectBase").ToArray();

            List<string> names = new();
            foreach (Type t in types)
            {
                var effect = Target.GetType().Assembly.CreateInstance(t.Name) as RPGEffectBase;

                names.Add(effect.GetName());
            }

            BeginVertical();

            Label("Добавление эффекта");

            selected = EditorGUILayout.Popup(selected, names.ToArray());

            if (Button("Добавить"))
                Target.Effects.Add(Target.GetType().Assembly.CreateInstance(types[selected].Name) as RPGEffectBase);


            for (int i = 0; i < Target.Effects.Count; i++)
            {
                BeginVertical();

                Label(Target.Effects[i].GetName());

                effectEditorService.BuildGUI(Target.Effects[i]);

                if (GUILayout.Button("Удалить", GUILayout.Width(100), GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
                    Target.Effects.RemoveAt(i);

                EndVertical();
            }

            EndVertical();

            if (GuiChanged)
                EditorUtility.SetDirty(Target);
        }
    }
}