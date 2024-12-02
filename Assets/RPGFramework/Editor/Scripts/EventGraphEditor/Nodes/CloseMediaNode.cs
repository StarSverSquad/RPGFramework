using UnityEngine.UIElements;

[UseActionNode]
public class CloseMediaNode : ActionNodeWrapper<CloseMediaAction>
{
    public CloseMediaNode(CloseMediaAction Action) : base(Action)
    {
    }

    public override void UIContructor()
    {
        FloatField fadeTimeField = new FloatField("Время затухания");

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

        extensionContainer.Add(fadeTimeField);
        extensionContainer.Add(isWaitToggle);
    }
}