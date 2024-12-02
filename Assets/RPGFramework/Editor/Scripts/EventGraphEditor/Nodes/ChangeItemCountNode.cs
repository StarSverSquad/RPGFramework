using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[UseActionNode]
public class ChangeItemCountNode : ActionNodeWrapper<ChangeItemCountAction>
{
    public ChangeItemCountNode(ChangeItemCountAction Action) : base(Action)
    {
    }

    public override void UIContructor()
    {
        ObjectField itemField = new ObjectField("Предмет")
        {
            allowSceneObjects = false,
            objectType = typeof(RPGCollectable),
        };

        itemField.SetValueWithoutNotify(Action.Item);
        itemField.RegisterValueChangedCallback(value =>
        {
            Action.Item = (RPGCollectable)value.newValue;

            MakeDirty();
        });

        VisualElement horizontal = new VisualElement();

        horizontal.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
        horizontal.style.justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween);

        Label label = new Label("Количество");

        PopupField<int> enumField = new PopupField<int>(new List<int> { 0, 1 }, 
                                                        0, 
                                                        (value) =>
                                                        {
                                                            return value switch
                                                            {
                                                                0 => "Добавить",
                                                                1 => "Назначить",
                                                                _ => "UNKNOWN"
                                                            };
                                                        },
                                                        (value) =>
                                                        {
                                                            return value switch
                                                            {
                                                                0 => "Добавить",
                                                                1 => "Назначить",
                                                                _ => "UNKNOWN"
                                                            };
                                                        });

        enumField.SetValueWithoutNotify(Action.IsSet ? 1 : 0);
        enumField.RegisterValueChangedCallback(value =>
        {
            Action.IsSet = value.newValue != 0;

            MakeDirty();
        });

        IntegerField countField = new IntegerField();

        countField.SetValueWithoutNotify(Action.Count);
        countField.RegisterValueChangedCallback(value =>
        {
            Action.Count = value.newValue;

            MakeDirty();
        });

        horizontal.Add(label);
        horizontal.Add(enumField);
        horizontal.Add(countField);

        extensionContainer.Add(itemField);
        extensionContainer.Add(horizontal);
    }
}
