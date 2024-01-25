using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class SetActiveNode : ActionNodeBase
{
    public SetActiveNode(SetActiveAction action) : base(action)
    {
    }

    public override void UIContructor()
    {
        SetActiveAction act = action as SetActiveAction;

        Toggle activeToggle = new Toggle("Включить?");

        activeToggle.SetValueWithoutNotify(act.setActive);
        activeToggle.RegisterValueChangedCallback(data =>
        {
            act.setActive = data.newValue;

            MakeDirty();
        });


        extensionContainer.Add(activeToggle);

        ObjectField gameObject = new ObjectField("Объект")
        {
            objectType = typeof(GameObject),
            allowSceneObjects = true
        };

        gameObject.SetValueWithoutNotify(act.gameObject);
        gameObject.RegisterValueChangedCallback(data =>
        {
            act.gameObject = data.newValue as GameObject;

            MakeDirty();
        });

        extensionContainer.Add(gameObject);
    }
}
