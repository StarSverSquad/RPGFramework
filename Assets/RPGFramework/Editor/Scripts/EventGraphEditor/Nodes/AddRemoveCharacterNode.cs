﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class AddRemoveCharacterNode : ActionNodeWrapper<AddRemoveCharacterAction>
{
    public AddRemoveCharacterNode(AddRemoveCharacterAction Action) : base(Action)
    {
    }

    public override void UIContructor()
    {
        Toggle isAddToggle = new Toggle("Добавить?");

        isAddToggle.SetValueWithoutNotify(Action.isAdd);
        isAddToggle.RegisterValueChangedCallback(data =>
        {
            Action.isAdd = data.newValue;

            MakeDirty();
        });

        extensionContainer.Add(isAddToggle);

        Toggle updateModelsToggle = new Toggle("Обнавить модели?");

        updateModelsToggle.SetValueWithoutNotify(Action.updateModels);
        updateModelsToggle.RegisterValueChangedCallback(data =>
        {
            Action.updateModels = data.newValue;

            MakeDirty();

            UpdateUI();
        });

        extensionContainer.Add(updateModelsToggle);

        ObjectField characterField = new ObjectField("Персонаж")
        {
            objectType = typeof(RPGCharacter),
            allowSceneObjects = false
        };

        characterField.SetValueWithoutNotify(Action.character);
        characterField.RegisterValueChangedCallback(data =>
        {
            Action.character = data.newValue as RPGCharacter;

            MakeDirty();
        });

        extensionContainer.Add(characterField);

        if (!Action.updateModels)
        {
            ObjectField exitentObjectField = new ObjectField("Существующая модель")
            {
                objectType = typeof(DynamicExplorerObject),
                allowSceneObjects = true
            };

            exitentObjectField.SetValueWithoutNotify(Action.existentObject);
            exitentObjectField.RegisterValueChangedCallback(data =>
            {
                Action.existentObject = data.newValue as DynamicExplorerObject;

                MakeDirty();
            });

            extensionContainer.Add(exitentObjectField);
        }
    }
}
