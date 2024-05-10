using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class InvokeEventNode : ActionNodeBase
{
    public InvokeEventNode(InvokeEventAction action) : base(action)
    {
    }

    public override void UIContructor()
    {
        InvokeEventAction se = action as InvokeEventAction;

        ObjectField objectField = new ObjectField("—обытие")
        {
            allowSceneObjects = true,
            objectType = typeof(ExplorerEvent)
        };

        objectField.SetValueWithoutNotify(se.Event);
        objectField.RegisterValueChangedCallback(val =>
        {
            se.Event = (ExplorerEvent)val.newValue;

            MakeDirty();
        });

        Toggle toggle = new Toggle("∆дать?");

        toggle.SetValueWithoutNotify(se.Event);
        toggle.RegisterValueChangedCallback(val =>
        {
            se.Wait = val.newValue;

            MakeDirty();
        });

        extensionContainer.Add(objectField);
        extensionContainer.Add(toggle);
    }
}
