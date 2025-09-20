using System;
using System.Linq;
using UnityEditor;
using System.Collections.Generic;
using RPGF.Core.Location;
using RPGF;
using UnityEngine;

[CustomEditor(typeof(RpgfLocationInfo))]
public class LocationInfoEditor : Editor
{
    private RpgfLocationInfo location;

    private void OnEnable()
    {
        location = (RpgfLocationInfo)target;
    }

    public override void OnInspectorGUI()
    {
        location.Name = EditorGUILayout.TextField("Название", location.Name);

        EditorGUILayout.LabelField("Описание");
        location.Description = EditorGUILayout.TextArea(location.Description);

        location.CameraCapture = (MainCameraManager.CaptureType)EditorGUILayout.EnumPopup("Камера", location.CameraCapture);

        List<string> sceneNames = Resources.LoadAll<SceneAsset>("Scenes\\").Select(s => s.name).ToList();

        int locIndex = EditorGUILayout.Popup("Сцена", sceneNames.IndexOf(location.SceneName), sceneNames.ToArray());

        if (locIndex >= 0 && locIndex < sceneNames.Count)
            location.SceneName = sceneNames[locIndex];

        if (GUI.changed) 
            EditorUtility.SetDirty(location);
    }
}