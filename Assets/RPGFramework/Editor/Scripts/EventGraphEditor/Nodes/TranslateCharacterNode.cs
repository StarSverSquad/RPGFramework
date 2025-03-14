using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using RPGF.Character;
using RPGF;

[UseActionNode(contextualMenuPath: "������� ������������/����������� ���������")]
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

        Toggle InpartyToggle = new Toggle("�������� � ������");

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
            TextField TagField = new TextField("��� ���������");

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
            ObjectField CharacterField = new ObjectField("��������")
            {
                allowSceneObjects = true,
                objectType = typeof(PlayableCharacterModelController)
            };

            CharacterField.SetValueWithoutNotify(Action.CharacterInScene);
            CharacterField.RegisterValueChangedCallback(i =>
            {
                Action.CharacterInScene = (PlayableCharacterModelController)i.newValue;

                MakeDirty();
            });

            extensionContainer.Add(CharacterField);
        }

        switch (Action.Type)
        {
            case TranslateCharacterAction.TranslateType.Move:
            case TranslateCharacterAction.TranslateType.MoveRelative:
                {
                    Toggle ReplaceInstanceToggle = new Toggle("�����?");

                    ReplaceInstanceToggle.SetValueWithoutNotify(Action.ReplaceInstantly);
                    ReplaceInstanceToggle.RegisterValueChangedCallback(i =>
                    {
                        Action.ReplaceInstantly = i.newValue;

                        MakeDirty();
                        UpdateUI();
                    });

                    extensionContainer.Add(ReplaceInstanceToggle);

                    if (!Action.ReplaceInstantly)
                    {
                        FloatField SpeedField = new FloatField("��������");

                        SpeedField.SetValueWithoutNotify(Action.Speed);
                        SpeedField.RegisterValueChangedCallback(i =>
                        {
                            Action.Speed = i.newValue;

                            MakeDirty();
                            UpdateUI();
                        });

                        extensionContainer.Add(SpeedField);

                        Toggle WaitToggle = new Toggle("�����?");

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
                        Label OffsetLabel = new Label("�����������");

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
                        ObjectField TransofrmField = new ObjectField("�����")
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
                    EnumField DiretionEnum = new EnumField("�������", Action.Direction);

                    DiretionEnum.SetValueWithoutNotify(Action.Direction);
                    DiretionEnum.RegisterValueChangedCallback(i =>
                    {
                        Action.Direction = (ViewDirection)i.newValue;

                        MakeDirty();
                    });

                    extensionContainer.Add(DiretionEnum);
                }
                break;
        }
    }
}
