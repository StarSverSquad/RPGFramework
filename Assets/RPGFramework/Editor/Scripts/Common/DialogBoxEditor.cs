using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MessageBoxManager))]
public class DialogBoxEditor : Editor
{
    private MessageBoxManager manager;

    private void OnEnable()
    {
        manager = (MessageBoxManager)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
