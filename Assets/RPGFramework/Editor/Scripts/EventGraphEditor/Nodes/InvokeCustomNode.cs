using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[UseActionNode]
public class InvokeCustomNode : ActionNodeWrapper<InvokeCustomAction>
{
    public InvokeCustomNode(InvokeCustomAction Action) : base(Action)
    {
    }

    public override void UIContructor()
    {
        ObjectField objectField = new ObjectField("Событие")
        {
            allowSceneObjects = true,
            objectType = typeof(CustomActionBase)
        };

        objectField.SetValueWithoutNotify(Action.act);
        objectField.RegisterValueChangedCallback(i =>
        {
            Action.act = i.newValue as CustomActionBase;

            MakeDirty();
        });

        extensionContainer.Add(objectField);
    }
}