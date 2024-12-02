using UnityEngine.UIElements;

[UseActionNode]
public class SaveNode : ActionNodeWrapper<SaveAction>
{
    public SaveNode(SaveAction Action) : base(Action)
    {
    }

    public override void UIContructor()
    {
        IntegerField SaveField = new IntegerField("Ячейка для сохранения");

        SaveField.SetValueWithoutNotify(Action.slotId);
        SaveField.RegisterValueChangedCallback(vol =>
        {
            Action.slotId = (vol.newValue <= 0) ? 0 : vol.newValue;

            MakeDirty();
        });

        extensionContainer.Add(SaveField);
    }
}
