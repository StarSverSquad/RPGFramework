using RPGF.EventSystem.Graph;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPGF.Editor.EventSystem
{
    public class EventGraphWindow : EditorWindow
    {
        public GraphEvent Event { get; private set; }

        private Object eventContainer;

        private EventGraphView graphView;
        private Toolbar toolbar;

        public static void Initialize(GraphEvent @event, Object eventContainer)
        {
            EventGraphWindow win = GetWindow<EventGraphWindow>("Graph event editor", true);

            win.Event = @event;
            win.eventContainer = eventContainer;
            win.Initialize();
        }

        public void Initialize()
        {
            rootVisualElement.Clear();

            graphView = new EventGraphView(this);
            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);

            toolbar = new Toolbar();

            Button sch = new(Save)
            {
                text = "Сохранить изменения"
            };

            toolbar.Add(sch);

            rootVisualElement.Add(toolbar);

            graphView.OnSaved += GraphView_OnSaved;

            EditorSceneManager.sceneSaving += EditorSceneManager_sceneSaving;
        }

        private void EditorSceneManager_sceneSaving(UnityEngine.SceneManagement.Scene scene, string path)
        {
            graphView.SaveGraph();
            MarkDirty();
        }

        private void GraphView_OnSaved()
        {
            if (titleContent.text.Last() == '*')
                titleContent.text = titleContent.text.Split('*')[0];
        }

        public void MarkDirty()
        {
            EditorUtility.SetDirty(eventContainer);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            if (titleContent.text.Last() != '*')
                titleContent.text = $"{titleContent.text}*";
        }

        private void OnDisable()
        {
            EditorSceneManager.sceneSaving -= EditorSceneManager_sceneSaving;
        }

        private void Save()
        {
            graphView.SaveGraph();
        }
    }
}