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

        Toggle toggle = new Toggle("Добавить?");

        toggle.SetValueWithoutNotify(act.isAdd);
        toggle.RegisterValueChangedCallback(isAdd =>
        {
            act.isAdd = isAdd.newValue;

            MakeDirty();
        });

        extensionContainer.Add(toggle);

        ObjectField characterField = new ObjectField("Персонаж")
        {
            objectType = typeof(RPGCharacter),
            allowSceneObjects = false
        };

        characterField.SetValueWithoutNotify(act.character);
        characterField.RegisterValueChangedCallback(character =>
        {
            act.character = character.newValue as RPGCharacter;

            MakeDirty();
        });

        extensionContainer.Add(characterField);
    }
}
