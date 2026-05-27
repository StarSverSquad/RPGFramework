using UnityEngine;

namespace RPGF.Core.Location
{
    [CreateAssetMenu(fileName = "Location", menuName = "RPGFramework/Location")]
    public class RpgfLocationInfo : ScriptableObject
    {
        public string Tag;

        public MainCameraManager.CaptureType CameraCapture;

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