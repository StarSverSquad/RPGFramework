using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace RPGF.Core.Location
{
    [CreateAssetMenu(fileName = "Location", menuName = "RPGFramework/Location")]
    public class RpgfLocationInfo : ScriptableObject
    {
        [InfoBox("Для локализации используйте теги в формате LOC_{tag}_Name и LOC_{tag}_Description")]
        public string Tag;
        [Space]
        public MainCameraManager.CaptureType CameraCapture;
        [Space]
        [Scene]
        public string SceneName;

        public static string GetLocaleNameTag(string tag)
        {
            return $"LOC_{tag}_Name";
        }

        public static string GetLocaleDescriptionTag(string tag)
        {
            return $"LOC_{tag}_Description";
        }
    }
}