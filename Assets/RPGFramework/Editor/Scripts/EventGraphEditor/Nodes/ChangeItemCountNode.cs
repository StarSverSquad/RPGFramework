using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ChangeItemCountNode : ActionNodeBase
{
    public ChangeItemCountNode(ChangeItemCountAction action) : base(action)
    {
    }

    public override void UIContructor()
    {
        ChangeItemCountAction act = (ChangeItemCountAction)action;

        ObjectField itemField = new ObjectField("Предмет")
        {
            allowSceneObjects = false,
            objectType = typeof(RPGCollectable),
        };

        itemField.SetValueWithoutNotify(act.Item);
        itemField.RegisterValueChangedCallback(value =>
        {
            act.Item = (RPGCollectable)value.newValue;

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

        enumField.SetValueWithoutNotify(act.IsSet ? 1 : 0);
        enumField.RegisterValueChangedCallback(value =>
        {
            act.IsSet = value.newValue != 0;

            MakeDirty();
        });

        IntegerField countField = new IntegerField();

        countField.SetValueWithoutNotify(act.Count);
        countField.RegisterValueChangedCallback(value =>
        {
            act.Count = value.newValue;

            MakeDirty();
        });

        horizontal.Add(label);
        horizontal.Add(enumField);
        horizontal.Add(countField);

        extensionContainer.Add(itemField);
        extensionContainer.Add(horizontal);
    }
}
