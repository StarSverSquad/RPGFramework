using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class MessageNode : ActionNodeBase
{
    public MessageNode(MessageAction action) : base(action)
    {
    }

    public override void UIContructor()
    {
        MessageAction dialog = action as MessageAction;

        Label txtLabel = new Label("Сообщение");

        TextField textField = new TextField()
        {
            multiline = true,
            tooltip = "Комманды:\n" +
                      "\\<[color]=HEXCOLOR>(text)</[color]> - установка цвета (без [])\n" +
                      "<\\(. или : или |)> - ждать 0.25с, 0.5с, 1с"
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
            tooltip = "Символов в секунду\n" +
                      "Если 0 то будет выставленно стандартное значение"
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

        EnumField positionField = new EnumField("Позиция", MessageBoxManager.DialogBoxPosition.Bottom);

        positionField.SetValueWithoutNotify(dialog.message.position);
        positionField.RegisterValueChangedCallback(position =>
        {
            dialog.message.position = Enum.Parse<MessageBoxManager.DialogBoxPosition>(position.newValue.ToString());

            MakeDirty();
        });

        ObjectField textEffectField = new ObjectField("Эффект текста")
        {
            allowSceneObjects = true,
            objectType = typeof(TextVisualEffectBase)
        };

        List<string> types = new List<string>() { "None" };

        types.AddRange(action.GetType().Assembly.GetTypes()
            .Where(i => i.BaseType != null && i.BaseType.Name == "TextVisualEffectBase")
            .Select(i => i.Name));

        int index = types.IndexOf(dialog.message.textEffectTypeName);

        PopupField<string> effectPopup = new PopupField<string>("Эффект", types, index < 0 ? 0 : index);

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
}