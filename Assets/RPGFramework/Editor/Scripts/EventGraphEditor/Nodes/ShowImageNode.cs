using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ShowImageNode : ActionNodeBase
{
    public ShowImageNode(ShowImageAction action) : base(action)
    {
    }

    public override void UIContructor()
    {
        ShowImageAction act = action as ShowImageAction;

        ObjectField spriteField = new ObjectField("Изображение")
        {
            allowSceneObjects = false,
            objectType = typeof(Sprite),
        };

        spriteField.SetValueWithoutNotify(act.ImageSprite);
        spriteField.RegisterValueChangedCallback(value =>
        {
            act.ImageSprite = (Sprite)value.newValue;

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

        extensionContainer.Add(spriteField);
        extensionContainer.Add(fadeTimeField);
        extensionContainer.Add(isWaitToggle);
    }
}