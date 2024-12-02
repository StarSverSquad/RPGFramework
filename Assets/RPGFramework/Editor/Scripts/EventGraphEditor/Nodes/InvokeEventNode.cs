using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
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
            objectType = typeof(ExplorerEvent)
        };

        objectField.SetValueWithoutNotify(Action.Event);
        objectField.RegisterValueChangedCallback(val =>
        {
            Action.Event = (ExplorerEvent)val.newValue;

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
