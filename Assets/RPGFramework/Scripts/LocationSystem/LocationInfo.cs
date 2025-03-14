using UnityEngine;

[CreateAssetMenu(fileName = "Location", menuName = "RPGFramework/Location")]
public class LocationInfo : ScriptableObject
{
    public string Name;
    public string Description;

    public MainCameraManager.CaptureType CameraCapture;

    public string SceneName;
}