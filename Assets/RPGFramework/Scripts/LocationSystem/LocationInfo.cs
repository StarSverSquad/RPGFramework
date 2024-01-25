using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Location", menuName = "RPGFramework/Location")]
public class LocationInfo : ScriptableObject
{
    public string Name;
    public string Description;

    public MainCameraManager.CameraLink CameraLink;

    public string SceneName;
}