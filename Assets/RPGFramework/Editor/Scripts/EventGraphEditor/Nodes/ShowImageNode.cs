using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ShowImageNode : ActionNodeWrapper<ShowImageAction>
{
    public ShowImageNode(ShowImageAction Action) : base(Action)
    {
    }

    public override void UIContructor()
    {
        ObjectField spriteField = new ObjectField("Изображение")
        {
            allowSceneObjects = false,
            objectType = typeof(Sprite),
        };

        spriteField.SetValueWithoutNotify(Action.ImageSprite);
        spriteField.RegisterValueChangedCallback(value =>
        {
            Action.ImageSprite = (Sprite)value.newValue;

            MakeDirty();
        });

        FloatField fadeTimeField = new FloatField("Время появления");

        fadeTimeField.SetValueWithoutNotify(Action.FadeTime);
        fadeTimeField.RegisterValueChangedCallback(value =>
        {
            if (value.newValue < 0)
            {
                fadeTimeField.SetValueWithoutNotify(0);
                Action.FadeTime = 0;
            }
            else
                Action.FadeTime = value.newValue;

            MakeDirty();
        });

        Toggle isWaitToggle = new Toggle("Ждать?");

        isWaitToggle.SetValueWithoutNotify(Action.IsWait);
        isWaitToggle.RegisterValueChangedCallback(value =>
        {
            Action.IsWait = value.newValue;

            MakeDirty();
        });

        extensionContainer.Add(spriteField);
        extensionContainer.Add(fadeTimeField);
        extensionContainer.Add(isWaitToggle);
    }
}