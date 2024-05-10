using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class MoveDEONode : ActionNodeBase
{
    public MoveDEONode(MoveDEOAction action) : base(action)
    {
    }

    public override void UIContructor()
    {
        MoveDEOAction act = action as MoveDEOAction;

        ObjectField modelField = new ObjectField("Моделька (DEO)")
        {
            allowSceneObjects = true,
            objectType = typeof(DynamicExplorerObject),
            tooltip = "Dynamic Explorer Object"
        };

        modelField.SetValueWithoutNotify(act.Model);
        modelField.RegisterValueChangedCallback(value =>
        {
            act.Model = (DynamicExplorerObject)value.newValue;

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