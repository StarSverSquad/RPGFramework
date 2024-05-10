using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class MoveCharacterNode : ActionNodeBase
{
    public MoveCharacterNode(MoveCharacterAction action) : base(action)
    {
    }

    public override void UIContructor()
    {
        MoveCharacterAction act = action as MoveCharacterAction;

        IntegerField modelField = new IntegerField("Индекс персонажа");

        modelField.SetValueWithoutNotify(act.Index);
        modelField.RegisterValueChangedCallback(value =>
        {
            if (act.Index < 0)
            {
                modelField.SetValueWithoutNotify(0);
                act.Index = 0;
            }
            else
                act.Index = value.newValue;

            MakeDirty();
        });

        Vector2Field offsetField = new Vector2Field("Расстояние");

        offsetField.SetValueWithoutNotify(act.Offset);
        offsetField.RegisterValueChangedCallback(value =>
        {
            act.Offset = value.newValue;

            MakeDirty();
        });

        FloatField speedField = new FloatField("Скорость");

        speedField.SetValueWithoutNotify(act.Speed);
        speedField.RegisterValueChangedCallback(value =>
        {
            act.Speed = value.newValue;

            MakeDirty();
        });

        Toggle isWaitToggle = new Toggle("Ждать?");

        isWaitToggle.SetValueWithoutNotify(act.IsWait);
        isWaitToggle.RegisterValueChangedCallback(value =>
        {
            act.IsWait = value.newValue;

            MakeDirty();
        });

        extensionContainer.Add(modelField);
        extensionContainer.Add(offsetField);
        extensionContainer.Add(speedField);
        extensionContainer.Add(isWaitToggle);
    }
}