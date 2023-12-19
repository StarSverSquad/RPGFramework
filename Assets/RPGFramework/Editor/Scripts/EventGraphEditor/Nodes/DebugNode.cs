using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DebugNode : ActionNodeBase
{
    public DebugNode(DebugAction action) : base(action)
    {
    }

    public override void UIContructor()
    {
        DebugAction da = action as DebugAction;

        PopupField<string> popupField = new PopupField<string>("Тип", Enum.GetNames(typeof(DebugAction.WarningLevelType)).ToList(), 0);

        popupField.SetValueWithoutNotify(da.WarningLevel.ToString());

        popupField.RegisterValueChangedCallback(i =>
        {
            da.WarningLevel = Enum.Parse<DebugAction.WarningLevelType>(i.newValue);

            MakeDirty();
        });

        TextField textField = new TextField("Текст")
        {
            multiline = true,
        };

        textField.SetValueWithoutNotify(da.ConsoleOutputText);

        textField.RegisterValueChangedCallback(i =>
        {
            da.ConsoleOutputText = i.newValue;

            MakeDirty();
        });

        extensionContainer.Add(popupField);
        extensionContainer.Add(textField);
    }
}