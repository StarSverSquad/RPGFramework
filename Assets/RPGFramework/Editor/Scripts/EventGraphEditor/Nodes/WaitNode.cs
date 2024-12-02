using UnityEngine.UIElements;

[UseActionNode]
public class WaitNode : ActionNodeWrapper<WaitAction>
{
    public WaitNode(WaitAction Action) : base(Action)
    {
    }

    public override void UIContructor()
    {
        FloatField timeField = new FloatField("Время (сек.):");

        timeField.SetValueWithoutNotify(Action.time);
        timeField.RegisterValueChangedCallback(time =>
        {
            Action.time = time.newValue;

            MakeDirty();
        });

        extensionContainer.Add(timeField);
    }
}
