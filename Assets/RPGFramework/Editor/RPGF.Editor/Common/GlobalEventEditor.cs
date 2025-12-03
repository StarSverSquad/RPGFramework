using RPGF.Editor.Core;
using RPGF.Editor.EventSystem;
using RPGF.EventSystem;
using UnityEditor;

namespace RPGF.Editor
{
    [CustomEditor(typeof(GlobalEvent))]
    public class GlobalEventEditor : RPGFrameworkEditor<GlobalEvent>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Button("Открыть редактор"))
                EventGraphWindow.Initialize(Target.InnerEvent, Target);
        }
    }
}