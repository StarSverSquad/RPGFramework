using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class EventGraphWindow : EditorWindow
{
    private GraphEvent gEvent;

    private EventGraphView graphView;
    private Toolbar toolbar;

    private VisualElement notSavedWarning;

    public static void Initialize(GraphEvent gEvent)
    {
        EventGraphWindow win = GetWindow<EventGraphWindow>(true, "Graph event editor", true);

        win.gEvent = gEvent;
        win.Init();
    }

    public void Init()
    {
        saveChangesMessage = "save me";

        rootVisualElement.Clear();

        graphView = new EventGraphView(gEvent);
        graphView.StretchToParentSize();

        rootVisualElement.Add(graphView);

        toolbar = new Toolbar();

        Button sch = new Button(Save)
        {
            text = "Save changes"
        };

        toolbar.Add(sch);

        rootVisualElement.Add(toolbar);

        notSavedWarning = new VisualElement();
        notSavedWarning.style.width = 15;
        notSavedWarning.style.height = 15;
        notSavedWarning.style.backgroundColor = Color.red;

        graphView.OnMakeDirty += GraphView_OnMakeDirty;
        graphView.OnSaved += GraphView_OnSaved;

        EditorSceneManager.sceneSaving += EditorSceneManager_sceneSaving;
    }

    private void EditorSceneManager_sceneSaving(UnityEngine.SceneManagement.Scene scene, string path)
    {
        graphView.SaveGraph();
    }

    private void GraphView_OnSaved()
    {
        if (toolbar.Contains(notSavedWarning))
            toolbar.Remove(notSavedWarning);
    }

    private void GraphView_OnMakeDirty()
    {
        toolbar.Add(notSavedWarning);
        EditorUtility.SetDirty(gEvent);
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        
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
