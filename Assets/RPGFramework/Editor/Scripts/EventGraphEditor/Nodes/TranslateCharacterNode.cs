using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class TranslateCharacterNode : ActionNodeBase
{
    public TranslateCharacterNode(TranslateCharacterAction action) : base(action)
    {
    }

    public override void UIContructor()
    {
        TranslateCharacterAction act = action as TranslateCharacterAction;

        EnumField TypeEnum = new EnumField(act.Type);

        TypeEnum.SetValueWithoutNotify(act.Type);
        TypeEnum.RegisterValueChangedCallback(i =>
        {
            act.Type = (TranslateCharacterAction.TranslateType)i.newValue;

            MakeDirty();
            UpdateUI();
        });

        extensionContainer.Add(TypeEnum);

        Toggle InpartyToggle = new Toggle("�������� � ������");

        InpartyToggle.SetValueWithoutNotify(act.InParty);
        InpartyToggle.RegisterValueChangedCallback(i =>
        {
            act.InParty = i.newValue;

            MakeDirty();
            UpdateUI();
        });

        extensionContainer.Add(InpartyToggle);

        if (act.InParty)
        {
            TextField TagField = new TextField("��� ���������");

            TagField.SetValueWithoutNotify(act.CharacterTag);
            TagField.RegisterValueChangedCallback(i =>
            {
                act.CharacterTag = i.newValue;

                MakeDirty();
            });

            extensionContainer.Add(TagField);
        }
        else
        {
            ObjectField CharacterField = new ObjectField("��������")
            {
                allowSceneObjects = true,
                objectType = typeof(DynamicExplorerObject)
            };

            CharacterField.SetValueWithoutNotify(act.CharacterInScene);
            CharacterField.RegisterValueChangedCallback(i =>
            {
                act.CharacterInScene = (DynamicExplorerObject)i.newValue;

                MakeDirty();
            });

            extensionContainer.Add(CharacterField);
        }

        switch (act.Type)
        {
            case TranslateCharacterAction.TranslateType.Move:
            case TranslateCharacterAction.TranslateType.MoveRelative:
                {
                    Toggle ReplaceInstanceToggle = new Toggle("�����?");

                    ReplaceInstanceToggle.SetValueWithoutNotify(act.ReplaceInstance);
                    ReplaceInstanceToggle.RegisterValueChangedCallback(i =>
                    {
                        act.ReplaceInstance = i.newValue;

                        MakeDirty();
                        UpdateUI();
                    });

                    extensionContainer.Add(ReplaceInstanceToggle);

                    if (!act.ReplaceInstance)
                    {
                        FloatField SpeedField = new FloatField("��������");

                        SpeedField.SetValueWithoutNotify(act.Speed);
                        SpeedField.RegisterValueChangedCallback(i =>
                        {
                            act.Speed = i.newValue;

                            MakeDirty();
                            UpdateUI();
                        });

                        extensionContainer.Add(SpeedField);

                        Toggle WaitToggle = new Toggle("�����?");

                        WaitToggle.SetValueWithoutNotify(act.Wait);
                        WaitToggle.RegisterValueChangedCallback(i =>
                        {
                            act.Wait = i.newValue;

                            MakeDirty();
                        });

                        extensionContainer.Add(WaitToggle);
                    }

                    if (act.Type == TranslateCharacterAction.TranslateType.MoveRelative)
                    {
                        Label OffsetLabel = new Label("�����������");

                        extensionContainer.Add(OffsetLabel);

                        Vector2Field OffsetField = new Vector2Field();

                        OffsetField.SetValueWithoutNotify(act.Offset);
                        OffsetField.RegisterValueChangedCallback(i =>
                        {
                            act.Offset = i.newValue;

                            MakeDirty();
                        });

                        extensionContainer.Add(OffsetField);
                    }
                    else
                    {
                        ObjectField TransofrmField = new ObjectField("�����")
                        {
                            allowSceneObjects = true,
                            objectType = typeof(Transform)
                        };

                        TransofrmField.SetValueWithoutNotify(act.Point);
                        TransofrmField.RegisterValueChangedCallback(i =>
                        {
                            act.Point = (Transform)i.newValue;

                            MakeDirty();
                        });

                        extensionContainer.Add(TransofrmField);
                    }
                }
                break;
            case TranslateCharacterAction.TranslateType.Rotate:
                {
                    EnumField DiretionEnum = new EnumField("�������", act.Direction);

                    DiretionEnum.SetValueWithoutNotify(act.Direction);
                    DiretionEnum.RegisterValueChangedCallback(i =>
                    {
                        act.Direction = (CommonDirection)i.newValue;

                        MakeDirty();
                    });

                    extensionContainer.Add(DiretionEnum);
                }
                break;
        }
    }
}
