using RPGF.Core.Attributes;
using UnityEditor;
using UnityEngine;

namespace RPGF.Editor.Core.Attributes
{
    [CustomPropertyDrawer(typeof(DisplayName))]
    public class DisplayNameDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = attribute as DisplayName;
            label.text = attr.Name;

            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}