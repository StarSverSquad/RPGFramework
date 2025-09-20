using RPGF.Editor.EventSystem;
using RPGF.Editor.EventSystem.Attributes;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[UseActionNode]
public class SetActiveNode : ActionNodeBase<SetActiveAction>
{
    public SetActiveNode(SetActiveAction Action) : base(Action)
    {
    }

    public override void UIContructor()
    {
        Toggle activeToggle = new Toggle("Включить?");

        activeToggle.SetValueWithoutNotify(Action.setActive);
        activeToggle.RegisterValueChangedCallback(data =>
        {
            Action.setActive = data.newValue;

            MakeDirty();
        });


        extensionContainer.Add(activeToggle);

        ObjectField gameObject = new ObjectField("Объект")
        {
            objectType = typeof(GameObject),
            allowSceneObjects = true
        };

        gameObject.SetValueWithoutNotify(Action.gameObject);
        gameObject.RegisterValueChangedCallback(data =>
        {
            Action.gameObject = data.newValue as GameObject;

            MakeDirty();
        });

        extensionContainer.Add(gameObject);
    }
}
