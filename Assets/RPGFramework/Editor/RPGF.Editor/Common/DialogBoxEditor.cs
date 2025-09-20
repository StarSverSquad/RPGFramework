using RPGF.Editor.Core;
using RPGF.Shared;
using UnityEditor;

namespace RPGF.Editor
{
    [CustomEditor(typeof(MessageBoxManager))]
    public class DialogBoxEditor : RPGFrameworkEditor<MessageBoxManager>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}