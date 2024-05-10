using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class InvokeAnimationNode : ActionNodeBase
{
    public InvokeAnimationNode(InvokeAnimationAction action) : base(action)
    {
    }

    public override void UIContructor()
    {
        InvokeAnimationAction act = (InvokeAnimationAction)action;

        ObjectField animatorField = new ObjectField("Аниматор")
        {
            allowSceneObjects = true,
            objectType = typeof(Animator),
        };

        animatorField.SetValueWithoutNotify(act.ObjectAnimator);
        animatorField.RegisterValueChangedCallback(value =>
        {
            act.ObjectAnimator = (Animator)value.newValue;

            MakeDirty();
        });

        TextField triggerField = new TextField("Триггер");

        triggerField.SetValueWithoutNotify(act.Trigger);
        triggerField.RegisterValueChangedCallback(value =>
        {
            act.Trigger = value.newValue;

            MakeDirty();
        });

        extensionContainer.Add(animatorField);
        extensionContainer.Add(triggerField);
    }
}
