using System.Linq;
using RPGF.Core.Location;
using RPGF.Editor.Core;
using UnityEditor;
using UnityEngine;

namespace RPGF.Editor
{
    [CustomEditor(typeof(RpgfLocationInfo))]
    public class LocationInfoEditor : RPGFrameworkEditor<RpgfLocationInfo>
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Для локализации используйте теги в формате LOC_{tag}_Name и LOC_{tag}_Description", MessageType.Info);

            Target.Tag = TextField("Тег", Target.Tag);

            Target.CameraCapture = EnumPopup("Камера", Target.CameraCapture);

            var sceneNames = Resources.LoadAll<SceneAsset>("Scenes\\").Select(s => s.name).ToList();

            var locationIndex = Popup("Сцена", sceneNames.IndexOf(Target.SceneName), sceneNames.ToArray());

            if (locationIndex >= 0 && locationIndex < sceneNames.Count)
                Target.SceneName = sceneNames[locationIndex];

            if (GuiChanged)
                EditorUtility.SetDirty(Target);
        }
    }
}