using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using RPGF.Character;
using RPGF;

[UseActionNode(contextualMenuPath: "События исследования/Перемещение персонажа")]
public class TranslateCharacterNode : ActionNodeWrapper<TranslateCharacterAction>
{
    public TranslateCharacterNode(TranslateCharacterAction Action) : base(Action)
    {
    }

    public override void UIContructor()
    {
        var TypeEnum = BuildEnumField(
            Action.Type,
            value => Action.Type = value,
            i =>
            {
                return i switch
                {
                    TranslateCharacterAction.TranslateType.Move => "Переместить",
                    TranslateCharacterAction.TranslateType.MoveRelative => "Относительное перемещение",
                    TranslateCharacterAction.TranslateType.Rotate => "Повернуть",
                    TranslateCharacterAction.TranslateType.RotateToPlayer => "Повернуть к игроку",
                    _ => "Unknown"
                };
            },
            updateUI: true
            );

        AddToExtensionContainer(TypeEnum);

        Toggle InpartyToggle = BuildToggle(
            Action.InParty,
            value => Action.InParty = value,
            "Персонаж в группе?",
            updateUI: true
            );

        AddToExtensionContainer(InpartyToggle);

        if (Action.InParty)
        {
            TextField TagField = BuildTextField(
                Action.CharacterTag,
                value => Action.CharacterTag = value,
                "Тег персонажа:"
                );

            AddToExtensionContainer(TagField);
        }
        else
        {
            ObjectField CharacterField = BuildObjectField(
                Action.CharacterInScene,
                value => Action.CharacterInScene = value,
                "Персонаж на сцене:"
                );

            AddToExtensionContainer(CharacterField);
        }

        switch (Action.Type)
        {
            case TranslateCharacterAction.TranslateType.Move:
            case TranslateCharacterAction.TranslateType.MoveRelative:
                {
                    Toggle ReplaceInstanceToggle = BuildToggle(
                        Action.ReplaceInstantly,
                        value => Action.ReplaceInstantly = value,
                        "Резко переместить?",
                        updateUI: true
                        );

                    var withRotationToggle = BuildToggle(
                        Action.WithRotation, 
                        value => Action.WithRotation = value,
                        "С вращением?"
                        );

                    AddToExtensionContainer(withRotationToggle);
                    AddToExtensionContainer(ReplaceInstanceToggle);

                    if (!Action.ReplaceInstantly)
                    {
                        FloatField timeField = BuildFloatField(
                            Action.Time,
                            value => Action.Time = value,
                            "Время перемещения:"
                            );

                        AddToExtensionContainer(timeField);

                        Toggle WaitToggle = BuildToggle(
                            Action.Wait,
                            value => Action.Wait = value,
                            "Ждать завершения?"
                            );

                        AddToExtensionContainer(WaitToggle);
                    }

                    if (Action.Type == TranslateCharacterAction.TranslateType.MoveRelative)
                    {
                        Label OffsetLabel = new Label("Смещение:");

                        Vector2Field OffsetField = BuildVector2Field(
                            Action.Offset,
                            value => Action.Offset = value
                            );

                        AddToExtensionContainer(OffsetLabel);
                        AddToExtensionContainer(OffsetField);
                    }
                    else
                    {
                        ObjectField TransofrmField = BuildObjectField(
                            Action.Point,
                            value => Action.Point = value,
                            "Точка перемещения:"
                            );

                        AddToExtensionContainer(TransofrmField);
                    }
                }
                break;
            case TranslateCharacterAction.TranslateType.Rotate:
                {
                    var DiretionEnum = BuildEnumField(
                        Action.Direction,
                        value => Action.Direction = value,
                        i =>
                        {
                            return i switch
                            {
                                ViewDirection.Down => "Вниз",
                                ViewDirection.Left => "Влево",
                                ViewDirection.Right => "Вправо",
                                ViewDirection.Up => "Вверх",
                                _ => "Unknown"
                            };
                        },
                        "Направление поворота:"
                        );

                    AddToExtensionContainer(DiretionEnum);
                }
                break;
        }
    }
}
