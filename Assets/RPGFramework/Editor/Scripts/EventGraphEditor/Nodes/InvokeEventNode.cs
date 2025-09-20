using RPGF.EventSystem;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[UseActionNode]
public class InvokeEventNode : ActionNodeWrapper<InvokeEventAction>
{
    public InvokeEventNode(InvokeEventAction Action) : base(Action)
    {
    }

    public override void UIContructor()
    {
        ObjectField objectField = new ObjectField("—обытие")
        {
            allowSceneObjects = true,
            objectType = typeof(LocationEvent)
        };

        objectField.SetValueWithoutNotify(Action.Event);
        objectField.RegisterValueChangedCallback(val =>
        {
            Action.Event = (LocationEvent)val.newValue;

            MakeDirty();
        });

        Toggle toggle = new Toggle("∆дать?");

        toggle.SetValueWithoutNotify(Action.Event);
        toggle.RegisterValueChangedCallback(val =>
        {
            Action.Wait = val.newValue;

            MakeDirty();
        });

        extensionContainer.Add(objectField);
        extensionContainer.Add(toggle);
    }
}
