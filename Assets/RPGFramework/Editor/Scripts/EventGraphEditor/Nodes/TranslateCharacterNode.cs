using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[UseActionNode]
public class TranslateCharacterNode : ActionNodeWrapper<TranslateCharacterAction>
{
    public TranslateCharacterNode(TranslateCharacterAction Action) : base(Action)
    {
    }

    public override void UIContructor()
    {
        EnumField TypeEnum = new EnumField(Action.Type);

        TypeEnum.SetValueWithoutNotify(Action.Type);
        TypeEnum.RegisterValueChangedCallback(i =>
        {
            Action.Type = (TranslateCharacterAction.TranslateType)i.newValue;

            MakeDirty();
            UpdateUI();
        });

        extensionContainer.Add(TypeEnum);

        Toggle InpartyToggle = new Toggle("Персонаж в партии");

        InpartyToggle.SetValueWithoutNotify(Action.InParty);
        InpartyToggle.RegisterValueChangedCallback(i =>
        {
            Action.InParty = i.newValue;

            MakeDirty();
            UpdateUI();
        });

        extensionContainer.Add(InpartyToggle);

        if (Action.InParty)
        {
            TextField TagField = new TextField("Тег персонажа");

            TagField.SetValueWithoutNotify(Action.CharacterTag);
            TagField.RegisterValueChangedCallback(i =>
            {
                Action.CharacterTag = i.newValue;

                MakeDirty();
            });

            extensionContainer.Add(TagField);
        }
        else
        {
            ObjectField CharacterField = new ObjectField("Персонаж")
            {
                allowSceneObjects = true,
                objectType = typeof(DynamicExplorerObject)
            };

            CharacterField.SetValueWithoutNotify(Action.CharacterInScene);
            CharacterField.RegisterValueChangedCallback(i =>
            {
                Action.CharacterInScene = (DynamicExplorerObject)i.newValue;

                MakeDirty();
            });

            extensionContainer.Add(CharacterField);
        }

        switch (Action.Type)
        {
            case TranslateCharacterAction.TranslateType.Move:
            case TranslateCharacterAction.TranslateType.MoveRelative:
                {
                    Toggle ReplaceInstanceToggle = new Toggle("Резко?");

                    ReplaceInstanceToggle.SetValueWithoutNotify(Action.ReplaceInstance);
                    ReplaceInstanceToggle.RegisterValueChangedCallback(i =>
                    {
                        Action.ReplaceInstance = i.newValue;

                        MakeDirty();
                        UpdateUI();
                    });

                    extensionContainer.Add(ReplaceInstanceToggle);

                    if (!Action.ReplaceInstance)
                    {
                        FloatField SpeedField = new FloatField("Скорость");

                        SpeedField.SetValueWithoutNotify(Action.Speed);
                        SpeedField.RegisterValueChangedCallback(i =>
                        {
                            Action.Speed = i.newValue;

                            MakeDirty();
                            UpdateUI();
                        });

                        extensionContainer.Add(SpeedField);

                        Toggle WaitToggle = new Toggle("Ждать?");

                        WaitToggle.SetValueWithoutNotify(Action.Wait);
                        WaitToggle.RegisterValueChangedCallback(i =>
                        {
                            Action.Wait = i.newValue;

                            MakeDirty();
                        });

                        extensionContainer.Add(WaitToggle);
                    }

                    if (Action.Type == TranslateCharacterAction.TranslateType.MoveRelative)
                    {
                        Label OffsetLabel = new Label("Перемещение");

                        extensionContainer.Add(OffsetLabel);

                        Vector2Field OffsetField = new Vector2Field();

                        OffsetField.SetValueWithoutNotify(Action.Offset);
                        OffsetField.RegisterValueChangedCallback(i =>
                        {
                            Action.Offset = i.newValue;

                            MakeDirty();
                        });

                        extensionContainer.Add(OffsetField);
                    }
                    else
                    {
                        ObjectField TransofrmField = new ObjectField("Точка")
                        {
                            allowSceneObjects = true,
                            objectType = typeof(Transform)
                        };

                        TransofrmField.SetValueWithoutNotify(Action.Point);
                        TransofrmField.RegisterValueChangedCallback(i =>
                        {
                            Action.Point = (Transform)i.newValue;

                            MakeDirty();
                        });

                        extensionContainer.Add(TransofrmField);
                    }
                }
                break;
            case TranslateCharacterAction.TranslateType.Rotate:
                {
                    EnumField DiretionEnum = new EnumField("Сторона", Action.Direction);

                    DiretionEnum.SetValueWithoutNotify(Action.Direction);
                    DiretionEnum.RegisterValueChangedCallback(i =>
                    {
                        Action.Direction = (CommonDirection)i.newValue;

                        MakeDirty();
                    });

                    extensionContainer.Add(DiretionEnum);
                }
                break;
        }
    }
}
