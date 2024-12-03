using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[UseActionNode(contextualMenuPath: "Диалог/Сообщение")]
public class MessageNode : ActionNodeWrapper<MessageAction>
{
    private List<TextVisualEffectBase> effectTypes;

    public MessageNode(MessageAction Action)
        : base(Action)
    {
        effectTypes = Action
            .GetType()
            .Assembly.GetTypes()
            .Where(i => i.BaseType != null && i.BaseType.Name == "TextVisualEffectBase")
            .Select(i =>
                Activator.CreateInstance(i, new object[] { null, null }) as TextVisualEffectBase
            )
            .ToList();
    }

    public override void UIContructor()
    {
        Label txtLabel = new Label("Сообщение");

        TextField textField = BuildTextField(
            Action.message.text, 
            newVal => Action.message.text = newVal,
            multiline: true,
            tooltip: 
                  "Комманды:\n"
                + "< color=[HEXCOLOR] >[...]< /color > - установка цвета\n"
                + "< size=[%] >[...]< /size > - установка размера\n"
                + "< \\(., :, |) > - ждать 0.25с, 0.5с, 1с\n"
                + "< ! > - пауза\n"
                + "< %[LOCALE TAG] > - локализация"
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

        Toggle clearToggle = BuildToggle(
            Action.message.clear,
            val => Action.message.clear = val,
            label: "Очистить?");

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

        int defaultVal = 0;
        if (Action.message.textEffectTypeName != "None")
            defaultVal =
                effectTypes.FindIndex(i => i.GetType().Name == Action.message.textEffectTypeName)+1;

        PopupField<string> effectPopup = BuildPopupField(
            defaultVal,
            new List<string>() { "None" }
                .Concat(effectTypes.Select(i => i.GetType().Name))
                .ToList(),
            val => Action.message.textEffectTypeName = val,
            FormatEffectText,
            label: "Текстовый эффект");

        AddToExtensionContainer(txtLabel);
        AddToExtensionContainer(textField);
        AddToExtensionContainer(nameField);
        AddToExtensionContainer(speedField);
        AddToExtensionContainer(waitToggle);
        AddToExtensionContainer(clearToggle);
        AddToExtensionContainer(closeToggle);
        AddToExtensionContainer(spriteField);
        AddToExtensionContainer(clipField);
        AddToExtensionContainer(positionField);
        AddToExtensionContainer(effectPopup);
    }

    private string FormatEffectText(string str)
    {
        if (str == "None")
            return "Нет";
        else
            return effectTypes.Find(i => i.GetType().Name == str).GetTittle();
    }
}
