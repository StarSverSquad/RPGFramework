using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[CustomEditor(typeof(LocationInfo))]
public class LocationInfoEditor : Editor
{
    private LocationInfo loc;

    private void OnEnable()
    {
        loc = (LocationInfo)target;
    }

    public override void OnInspectorGUI()
    {
        loc.Name = EditorGUILayout.TextField("Название", loc.Name);

        EditorGUILayout.LabelField("Описание");
        loc.Description = EditorGUILayout.TextArea(loc.Description);

        loc.CameraCapture = (MainCameraManager.CaptureType)EditorGUILayout.EnumPopup("Камера", loc.CameraCapture);

        List<string> sceneNames = Resources.LoadAll<SceneAsset>("Scenes\\").Select(s => s.name).ToList();

        int locIndex = EditorGUILayout.Popup("Сцена", sceneNames.IndexOf(loc.SceneName), sceneNames.ToArray());

        if (locIndex >= 0 && locIndex < sceneNames.Count)
            loc.SceneName = sceneNames[locIndex];

        if (GUI.changed) 
            EditorUtility.SetDirty(loc);
    }
}