using System.Collections;
using UnityEngine;

public class AddRemoveCharacterAction : GraphActionBase
{
    public bool isAdd;
    public bool updateModels;

    public RPGCharacter character;

    public DynamicExplorerObject existentObject;

    public AddRemoveCharacterAction() : base("AddRemoveCharacter")
    {
        character = null;
        existentObject = null;
        isAdd = true;
        updateModels = true;
    }

    public override IEnumerator ActionCoroutine()
    {
        if (isAdd)
            GameManager.Instance.character.AddCharacter(character);
        else
            GameManager.Instance.character.RemoveCharacter(character);

        if (updateModels)
            LocalManager.Instance.Character.UpdateModels();
        else if (existentObject != null)
        {
            if (isAdd)
                LocalManager.Instance.Character.AddModel(existentObject);
            else
                LocalManager.Instance.Character.RemoveModel(existentObject);
        }

        yield break;
    }

    public override string GetHeader()
    {
        return "Изменить состав команды";
    }

    public override string GetInfo()
    {
        return "Добавляет или же удаляет персонажа из пачки.";
    }
}