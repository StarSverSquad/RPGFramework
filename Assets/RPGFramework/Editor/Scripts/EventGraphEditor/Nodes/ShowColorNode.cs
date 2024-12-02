using UnityEditor.UIElements;
using UnityEngine.UIElements;

[UseActionNode]
public class ShowColorNode : ActionNodeWrapper<ShowColorAction>
{
    public ShowColorNode(ShowColorAction Action) : base(Action)
    {
    }

    public override void UIContructor()
    {
        ColorField colorField = new ColorField("Цвет");

        colorField.style.width = new StyleLength(10);

        colorField.SetValueWithoutNotify(Action.Color);
        colorField.RegisterValueChangedCallback(value =>
        {
            Action.Color = value.newValue;

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

        extensionContainer.Add(colorField);
        extensionContainer.Add(fadeTimeField);
        extensionContainer.Add(isWaitToggle);
    }
}