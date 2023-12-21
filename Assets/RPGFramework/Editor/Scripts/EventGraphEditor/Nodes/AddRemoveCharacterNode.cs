using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class AddRemoveCharacterNode : ActionNodeBase
{
    public AddRemoveCharacterNode(AddRemoveCharacterAction action) : base(action)
    {
    }

    public override void UIContructor()
    {
        AddRemoveCharacterAction act = action as AddRemoveCharacterAction;

        Toggle isAddToggle = new Toggle("Добавить?");

        isAddToggle.SetValueWithoutNotify(act.isAdd);
        isAddToggle.RegisterValueChangedCallback(data =>
        {
            act.isAdd = data.newValue;

            MakeDirty();
        });

        extensionContainer.Add(isAddToggle);

        Toggle updateModelsToggle = new Toggle("Обнавить модели?");

        updateModelsToggle.SetValueWithoutNotify(act.updateModels);
        updateModelsToggle.RegisterValueChangedCallback(data =>
        {
            act.updateModels = data.newValue;

            MakeDirty();

            UpdateUI();
        });

        extensionContainer.Add(updateModelsToggle);

        ObjectField characterField = new ObjectField("Персонаж")
        {
            objectType = typeof(RPGCharacter),
            allowSceneObjects = false
        };

        characterField.SetValueWithoutNotify(act.character);
        characterField.RegisterValueChangedCallback(data =>
        {
            act.character = data.newValue as RPGCharacter;

            MakeDirty();
        });

        extensionContainer.Add(characterField);

        if (!act.updateModels)
        {
            ObjectField exitentObjectField = new ObjectField("Существующая модель")
            {
                objectType = typeof(DynamicExplorerObject),
                allowSceneObjects = true
            };

            exitentObjectField.SetValueWithoutNotify(act.existentObject);
            exitentObjectField.RegisterValueChangedCallback(data =>
            {
                act.existentObject = data.newValue as DynamicExplorerObject;

                MakeDirty();
            });

            extensionContainer.Add(exitentObjectField);
        }
    }
}
