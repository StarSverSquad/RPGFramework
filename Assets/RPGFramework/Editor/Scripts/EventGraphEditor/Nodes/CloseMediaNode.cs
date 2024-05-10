using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class CloseMediaNode : ActionNodeBase
{
    public CloseMediaNode(CloseMediaAction action) : base(action)
    {
    }

    public override void UIContructor()
    {
        CloseMediaAction act = action as CloseMediaAction;

        FloatField fadeTimeField = new FloatField("Время затухания");

        fadeTimeField.SetValueWithoutNotify(act.FadeTime);
        fadeTimeField.RegisterValueChangedCallback(value =>
        {
            if (value.newValue < 0)
            {
                fadeTimeField.SetValueWithoutNotify(0);
                act.FadeTime = 0;
            }
            else
                act.FadeTime = value.newValue;

            MakeDirty();
        });

        Toggle isWaitToggle = new Toggle("Ждать?");

        isWaitToggle.SetValueWithoutNotify(act.IsWait);
        isWaitToggle.RegisterValueChangedCallback(value =>
        {
            act.IsWait = value.newValue;

            MakeDirty();
        });

        extensionContainer.Add(fadeTimeField);
        extensionContainer.Add(isWaitToggle);
    }
}