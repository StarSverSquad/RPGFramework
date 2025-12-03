using RPGF.Editor.Core;
using RPGF.Editor.EventSystem;
using RPGF.EventSystem;
using RPGF.EventSystem.Graph;
using UnityEditor;
using UnityEngine;

namespace RPGF.Editor
{
    [CustomEditor(typeof(LocationEvent))]
    public class ExplorerEventEditor : RPGFrameworkEditor<LocationEvent>
    {
        private void OnEnable()
        {
            Target.InnerEvent ??= new GraphEvent();
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Настройки");

            Target.Interaction = EnumPopup("Тип взаимодействия", Target.Interaction);

            Target.OnlyOne = Toggle("Только раз?:", Target.OnlyOne);
            Target.Parallel = Toggle("Паралельно?:", Target.Parallel);

            if (Target.InnerEvent != null && Button("Открыть редактор"))
                EventGraphWindow.Initialize(Target.InnerEvent, Target);

            if (Button("Пересоздать"))
                Target.InnerEvent = new GraphEvent();

            if (GuiChanged)
                EditorUtility.SetDirty(Target);
        }
    }
}