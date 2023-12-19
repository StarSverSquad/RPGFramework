using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class InvokeCustomNode : ActionNodeBase
{
    public InvokeCustomNode(InvokeCustomAction action) : base(action)
    {
    }

    public override void UIContructor()
    {
        InvokeCustomAction ic = action as InvokeCustomAction;

        ObjectField objectField = new ObjectField("Событие")
        {
            allowSceneObjects = true,
            objectType = typeof(CustomActionBase)
        };

        objectField.SetValueWithoutNotify(ic.act);
        objectField.RegisterValueChangedCallback(i =>
        {
            ic.act = i.newValue as CustomActionBase;

            MakeDirty();
        });

        extensionContainer.Add(objectField);
    }
}