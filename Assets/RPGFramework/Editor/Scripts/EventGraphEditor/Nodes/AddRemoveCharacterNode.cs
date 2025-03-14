using RPGF.Character;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[UseActionNode]
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
                objectType = typeof(PlayableCharacterModelController),
                allowSceneObjects = true
            };

            exitentObjectField.SetValueWithoutNotify(Action.existentObject);
            exitentObjectField.RegisterValueChangedCallback(data =>
            {
                Action.existentObject = data.newValue as PlayableCharacterModelController;

                MakeDirty();
            });

            extensionContainer.Add(exitentObjectField);
        }
    }
}
