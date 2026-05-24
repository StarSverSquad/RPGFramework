using RPGF.Core.RPGEffect;
using RPGF.Core.RPGEffect.Attributes;
using RPGF.Editor.Core;
using RPGF.Editor.Core.Services;
using RPGF.RPG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace RPGF.Editor
{
    [CustomEditor(typeof(RPGConsumed))]
    public class RPGConsumedEditor : RPGFrameworkEditor<RPGConsumed>
    {
        private EffectEditorService _effectEditorService;
        private int selected = 0;

        private void OnEnable()
        {
            _effectEditorService = new EffectEditorService();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Type[] types = Target.GetType().Assembly.GetTypes()
                .Where(
                    i => i.BaseType == typeof(RPGEffectBase) 
                    && i.GetCustomAttribute<UseRPGEffectAttribute>() != null)
                .ToArray();

            List<string> names = new();
            foreach (Type t in types)
            {
                var meta = t.GetCustomAttribute<UseRPGEffectAttribute>();
                names.Add(meta.Label);
            }

            BeginVertical();

            Label("Добавление эффекта");

            selected = EditorGUILayout.Popup(selected, names.ToArray());

            if (Button("Добавить"))
                Target.Effects.Add(Activator.CreateInstance(types[selected]) as RPGEffectBase);


            for (int i = 0; i < Target.Effects.Count; i++)
            {
                BeginVertical();

                var meta = Target.Effects[i].GetType().GetCustomAttribute<UseRPGEffectAttribute>();

                Label(meta.Label);

                _effectEditorService.BuildGUI(Target.Effects[i]);

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