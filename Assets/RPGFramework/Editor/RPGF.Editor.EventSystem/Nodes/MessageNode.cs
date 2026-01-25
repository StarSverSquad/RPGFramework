using RPGF.Actions;
using RPGF.Editor.EventSystem.Attributes;
using RPGF.Shared;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace RPGF.Editor.EventSystem.Nodes
{
    [UseActionNode("Сообщение", contextualMenuPath: "Диалог/Сообщение")]
    public class MessageNode : ActionNodeBase<MessageAction>
    {
        public MessageNode(MessageAction Action)
            : base(Action)
        {
        }

        public override void UIContructor()
        {
            Label txtLabel = new Label("Сообщение");

            TextField textField = BuildTextField(
                Action.message.text,
                newVal => Action.message.text = newVal,
                multiline: true
            );

            TextField nameField = BuildTextField(
                Action.message.name,
                val => Action.message.name = val,
                label: "Имя");

            FloatField speedField = BuildFloatField(
                Action.message.speed,
                val => Action.message.speed = val,
                tooltip: "Символов в секунду\n" + "Если 0 то будет выставленно стандартное значение",
                label: "Скорость текста");

            Toggle waitToggle = BuildToggle(
                Action.message.wait,
                val => Action.message.wait = val,
                label: "Ждать?");

            Toggle closeToggle = BuildToggle(
                Action.message.closeWindow,
                val => Action.message.closeWindow = val,
                label: "Закрыть?");

            ObjectField spriteField = BuildObjectField(
                Action.message.image,
                val => Action.message.image = val,
                label: "Изображение");

            ObjectField clipField = BuildObjectField(
                Action.message.letterSound,
                val => Action.message.letterSound = val,
                label: "Звук букв");

            EnumField positionField = BuildEnumField(
                MessageBoxManager.DialogBoxPosition.Bottom,
                val => Action.message.position = val,
                label: "Позиция");

            AddToExtensionContainer(txtLabel);
            AddToExtensionContainer(textField);
            AddToExtensionContainer(nameField);
            AddToExtensionContainer(speedField);
            AddToExtensionContainer(waitToggle);
            AddToExtensionContainer(closeToggle);
            AddToExtensionContainer(spriteField);
            AddToExtensionContainer(clipField);
            AddToExtensionContainer(positionField);
        }
    }
}
