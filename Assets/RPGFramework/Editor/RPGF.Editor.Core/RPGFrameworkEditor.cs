using System;
using UnityEditor;
using UnityEngine;

namespace RPGF.Editor.Core
{
    public class RPGFrameworkEditor<T> : UnityEditor.Editor
        where T : UnityEngine.Object
    {
        protected T Target => target as T;

        protected bool GuiChanged => UnityEngine.GUI.changed;

        protected bool Button(string title)
        {
            return GUILayout.Button(title);
        }

        protected bool Toggle(string title, bool defaultValue)
        {
            return EditorGUILayout.Toggle(title, defaultValue);
        }

        protected PT EnumPopup<PT>(string title, PT defaultValue)
            where PT : Enum
        {
            return (PT)EditorGUILayout.EnumPopup(title, defaultValue);
        }

        protected int Popup(string title, int defaultIndex, params string[] names)
        {
            return EditorGUILayout.Popup(title, defaultIndex, names);
        }

        protected int Popup(int defaultIndex, params string[] names)
        {
            return EditorGUILayout.Popup(defaultIndex, names);
        }

        protected string TextArea(string defaultValue)
        {
            return EditorGUILayout.TextArea(defaultValue);
        }

        protected string TextField(string title, string defaultValue)
        {
            return EditorGUILayout.TextField(title, defaultValue);
        }

        protected void Label(string label)
        {
            EditorGUILayout.LabelField(label);
        }

        protected OT ObjectField<OT>(string title, OT defaultValue, bool allowSceneObject = false)
            where OT : UnityEngine.Object
        {
            return (OT)EditorGUILayout.ObjectField(title, defaultValue, typeof(OT), allowSceneObject);
        }

        protected void BeginVertical()
        {
            EditorGUILayout.BeginVertical(UnityEngine.GUI.skin.box);
        }

        protected void EndVertical()
        {
            EditorGUILayout.EndVertical();
        }
    }
}
