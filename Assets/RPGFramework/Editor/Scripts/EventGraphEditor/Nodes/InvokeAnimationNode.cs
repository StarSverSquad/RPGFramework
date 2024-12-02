using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[UseActionNode]
public class InvokeAnimationNode : ActionNodeWrapper<InvokeAnimationAction>
{
    public InvokeAnimationNode(InvokeAnimationAction Action) : base(Action)
    {
    }

    public override void UIContructor()
    {
        ObjectField animatorField = new ObjectField("Аниматор")
        {
            allowSceneObjects = true,
            objectType = typeof(Animator),
        };

        animatorField.SetValueWithoutNotify(Action.ObjectAnimator);
        animatorField.RegisterValueChangedCallback(value =>
        {
            Action.ObjectAnimator = (Animator)value.newValue;

            MakeDirty();
        });

        TextField triggerField = new TextField("Триггер");

        triggerField.SetValueWithoutNotify(Action.Trigger);
        triggerField.RegisterValueChangedCallback(value =>
        {
            Action.Trigger = value.newValue;

            MakeDirty();
        });

        extensionContainer.Add(animatorField);
        extensionContainer.Add(triggerField);
    }
}
