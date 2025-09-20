using RPGF.Editor.Core;
using RPGF.Editor.EventSystem;
using RPGF.EventSystem;
using UnityEditor;

namespace RPGF.Editor
{
    [CustomEditor(typeof(GraphEvent))]
    public class GraphEventScriptableEditor : RPGFrameworkEditor<GraphEvent>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Button("Открыть редактор"))
                EventGraphWindow.Initialize(Target);
        }
    }
}