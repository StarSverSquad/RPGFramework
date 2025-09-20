using System.Linq;
using UnityEditor;
using RPGF.Core.Location;
using UnityEngine;
using RPGF.Editor.Core;

namespace RPGF.Editor
{
    [CustomEditor(typeof(RpgfLocationInfo))]
    public class LocationInfoEditor : RPGFrameworkEditor<RpgfLocationInfo>
    {
        public override void OnInspectorGUI()
        {
            Target.Name = TextField("Название", Target.Name);

            Label("Описание");
            Target.Description = TextArea(Target.Description);

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