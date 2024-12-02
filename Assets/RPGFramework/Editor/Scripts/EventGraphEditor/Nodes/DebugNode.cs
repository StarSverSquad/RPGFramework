using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DebugNode : ActionNodeWrapper<DebugAction>
{
    public DebugNode(DebugAction Action) : base(Action)
    {
    }

    public override void UIContructor()
    {
        PopupField<string> popupField = new PopupField<string>("Тип", Enum.GetNames(typeof(DebugAction.WarningLevelType)).ToList(), 0);

        popupField.SetValueWithoutNotify(Action.WarningLevel.ToString());

        popupField.RegisterValueChangedCallback(i =>
        {
            Action.WarningLevel = Enum.Parse<DebugAction.WarningLevelType>(i.newValue);

            MakeDirty();
        });

        TextField textField = new TextField("Текст")
        {
            multiline = true,
        };

        textField.SetValueWithoutNotify(Action.ConsoleOutputText);

        textField.RegisterValueChangedCallback(i =>
        {
            Action.ConsoleOutputText = i.newValue;

            MakeDirty();
        });

        extensionContainer.Add(popupField);
        extensionContainer.Add(textField);
    }
}