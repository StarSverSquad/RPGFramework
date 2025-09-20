using RPGF.Core.Localization;
using RPGF.Editor.Core;
using UnityEditor;
using UnityEngine;

namespace RPGF.Editor.Localization
{
    [CustomEditor(typeof(LocalizationSheet))]
    public class LocalizationSheetEditor : RPGFrameworkEditor<LocalizationSheet>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Открыть редактор"))
            {
                LocalizationEditorWindow.Create(Target).Show();
            }
        }
    }
}