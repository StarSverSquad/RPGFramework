using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class MessageNode : ActionNodeBase
{
    private List<TextVisualEffectBase> effectTypes;

    public MessageNode(MessageAction action)
        : base(action)
    {
        effectTypes = action
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
        MessageAction dialog = action as MessageAction;

        Label txtLabel = new Label("Сообщение");

        TextField textField = new TextField()
        {
            multiline = true,
            tooltip =
                "Комманды:\n"
                + "< color=[HEXCOLOR] >[...]< /color > - установка цвета\n"
                + "< size=[%] >[...]< /size > - установка размера\n"
                + "< \\(., :, |) > - ждать 0.25с, 0.5с, 1с\n"
                + "< ! > - пауза\n"
                + "< %[LOCALE TAG] > - локализация"
        };

        textField.SetValueWithoutNotify(dialog.message.text);
        textField.RegisterValueChangedCallback(text =>
        {
            dialog.message.text = text.newValue;

            MakeDirty();
        });

        TextField nameField = new TextField("Имя");

        nameField.SetValueWithoutNotify(dialog.message.name);
        nameField.RegisterValueChangedCallback(name =>
        {
            dialog.message.name = name.newValue;

            MakeDirty();
        });

        FloatField speedField = new FloatField("Скорость текста")
        {
            tooltip = "Символов в секунду\n" + "Если 0 то будет выставленно стандартное значение"
        };

        speedField.SetValueWithoutNotify(dialog.message.speed);
        speedField.RegisterValueChangedCallback(speed =>
        {
            dialog.message.speed = speed.newValue;

            MakeDirty();
        });

        Toggle waitToggle = new Toggle("Ждать?");
        Toggle clearToggle = new Toggle("Очистить?");
        Toggle closeToggle = new Toggle("Закрыть?");

        waitToggle.SetValueWithoutNotify(dialog.message.wait);
        waitToggle.RegisterValueChangedCallback(wait =>
        {
            dialog.message.wait = wait.newValue;

            MakeDirty();
        });

        clearToggle.SetValueWithoutNotify(dialog.message.clear);
        clearToggle.RegisterValueChangedCallback(clear =>
        {
            dialog.message.clear = clear.newValue;

            MakeDirty();
        });

        closeToggle.SetValueWithoutNotify(dialog.message.closeWindow);
        closeToggle.RegisterValueChangedCallback(close =>
        {
            dialog.message.closeWindow = close.newValue;

            MakeDirty();
        });

        ObjectField spriteField = new ObjectField("Изображение")
        {
            allowSceneObjects = true,
            objectType = typeof(Sprite)
        };
        ObjectField clipField = new ObjectField("Звук букв")
        {
            allowSceneObjects = true,
            objectType = typeof(AudioClip)
        };

        spriteField.SetValueWithoutNotify(dialog.message.image);
        spriteField.RegisterValueChangedCallback(sprite =>
        {
            dialog.message.image = sprite.newValue as Sprite;

            MakeDirty();
        });

        clipField.SetValueWithoutNotify(dialog.message.letterSound);
        clipField.RegisterValueChangedCallback(sound =>
        {
            dialog.message.letterSound = sound.newValue as AudioClip;

            MakeDirty();
        });

        EnumField positionField = new EnumField(
            "Позиция",
            MessageBoxManager.DialogBoxPosition.Bottom
        );

        positionField.SetValueWithoutNotify(dialog.message.position);
        positionField.RegisterValueChangedCallback(position =>
        {
            dialog.message.position = Enum.Parse<MessageBoxManager.DialogBoxPosition>(
                position.newValue.ToString()
            );

            MakeDirty();
        });

        ObjectField textEffectField = new ObjectField("Эффект текста")
        {
            allowSceneObjects = true,
            objectType = typeof(TextVisualEffectBase)
        };

        int defaultVal = 0;
        if (dialog.message.textEffectTypeName != "None")
            defaultVal =
                effectTypes.FindIndex(i => i.GetType().Name == dialog.message.textEffectTypeName)+1;

        PopupField<string> effectPopup = new PopupField<string>(
            "Эффект",
            new List<string>() { "None" }
                .Concat(effectTypes.Select(i => i.GetType().Name))
                .ToList(),
            defaultVal,
            FormatEffectText,
            FormatEffectText
        );

        effectPopup.RegisterValueChangedCallback(effect =>
        {
            dialog.message.textEffectTypeName = effect.newValue;

            MakeDirty();
        });

        extensionContainer.Add(txtLabel);
        extensionContainer.Add(textField);
        extensionContainer.Add(nameField);
        extensionContainer.Add(speedField);
        extensionContainer.Add(waitToggle);
        extensionContainer.Add(clearToggle);
        extensionContainer.Add(closeToggle);
        extensionContainer.Add(spriteField);
        extensionContainer.Add(clipField);
        extensionContainer.Add(positionField);
        extensionContainer.Add(effectPopup);
    }

    private string FormatEffectText(string str)
    {
        if (str == "None")
            return "Нет";
        else
            return effectTypes.Find(i => i.GetType().Name == str).GetTittle();
    }
}
