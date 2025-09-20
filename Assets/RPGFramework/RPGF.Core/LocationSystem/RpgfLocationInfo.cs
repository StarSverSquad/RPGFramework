using UnityEngine;

namespace RPGF.Core.Location
{
    [CreateAssetMenu(fileName = "Location", menuName = "RPGFramework/Location")]
    public class RpgfLocationInfo : ScriptableObject
    {
        public string Name;
        public string Description;

        public MainCameraManager.CaptureType CameraCapture;

        public string SceneName;
    }
}