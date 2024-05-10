using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class ShowColorNode : ActionNodeBase
{
    public ShowColorNode(ShowColorAction action) : base(action)
    {
    }

    public override void UIContructor()
    {
        ShowColorAction act = action as ShowColorAction;

        ColorField colorField = new ColorField("Цвет");

        colorField.SetValueWithoutNotify(act.Color);
        colorField.RegisterValueChangedCallback(value =>
        {
            act.Color = value.newValue;

            MakeDirty();
        });

        FloatField fadeTimeField = new FloatField("Время появления");

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

        extensionContainer.Add(colorField);
        extensionContainer.Add(fadeTimeField);
        extensionContainer.Add(isWaitToggle);
    }
}