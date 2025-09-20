using RPGF.Editor.Core;
using RPGF.Editor.EventSystem;
using RPGF.EventSystem;
using UnityEditor;
using UnityEngine;

namespace RPGF.Editor
{
    [CustomEditor(typeof(LocationEvent))]
    public class ExplorerEventEditor : RPGFrameworkEditor<LocationEvent>
    {
        private void OnEnable()
        {
            Target.InnerEvent ??= CreateInstance<GraphEvent>();
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Настройки");

            Target.Interaction = EnumPopup("Тип взаимодействия", Target.Interaction);

            Target.OnlyOne = Toggle("Только раз?:", Target.OnlyOne);
            Target.Parallel = Toggle("Паралельно?:", Target.Parallel);

            if (Target.InnerEvent != null && Button("Открыть редактор"))
                EventGraphWindow.Initialize(Target.InnerEvent);

            if (Button("Пересоздать"))
                Target.InnerEvent = CreateInstance<GraphEvent>();

            if (GuiChanged)
                EditorUtility.SetDirty(Target);
        }
    }
}