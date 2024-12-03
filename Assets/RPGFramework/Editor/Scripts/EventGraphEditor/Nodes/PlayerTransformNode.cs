using UnityEditor.UIElements;
using UnityEngine.UIElements;

[UseActionNode(contextualMenuPath: "События исследования/Перемещение игрока")]
public class PlayerTransformNode : ActionNodeWrapper<PlayerTranslateAction>
{
    public PlayerTransformNode(PlayerTranslateAction action) : base(action)
    {
    }

    public override void UIContructor()
    {
        EnumField typeField = BuildEnumField(
            Action.Type,
            val => Action.Type = val,
            updateUI: true);

        AddToExtensionContainer(typeField);

        Toggle moveInstanceToggle = BuildToggle(
                        Action.ReplaceInstance,
                        val => Action.ReplaceInstance = val,
                        updateUI: true,
                        label: "Резко?");

        AddToExtensionContainer(moveInstanceToggle);

        switch (Action.Type)
        {
            case PlayerTranslateAction.TranslateType.Move:
                ObjectField pointField = BuildObjectField(
                            Action.MovePoint,
                            val => Action.MovePoint = val,
                            label: "Точка");

                AddToExtensionContainer(pointField);
                break;
            case PlayerTranslateAction.TranslateType.MoveRelative:
                Label offsetLabel = new Label("Отступ");

                Vector2Field offsetField = BuildVector2Field(
                    Action.Offset,
                    val => Action.Offset = val);

                AddToExtensionContainer(offsetLabel);
                AddToExtensionContainer(offsetField);
                break;
            default:
                break;
        }

        if (!Action.ReplaceInstance)
        {
            FloatField speedField = BuildFloatField(
                        Action.Speed,
                        val => Action.Speed = val,
                        label: "Скорость");

            AddToExtensionContainer(speedField);

            Toggle waitToggle = BuildToggle(
                Action.Wait,
                val => Action.Wait = val,
                label: "Ждать?");

            AddToExtensionContainer(waitToggle);
        }
    }
}