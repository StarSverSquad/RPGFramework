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
            GameManager.Instance.characterManager.AddCharacter(character);
        else
            GameManager.Instance.characterManager.RemoveCharacter(character);

        if (updateModels)
            ExplorerManager.instance.characterManager.UpdateModels();
        else if (existentObject != null)
        {
            if (isAdd)
                ExplorerManager.instance.characterManager.AddModel(existentObject);
            else
                ExplorerManager.instance.characterManager.RemoveModel(existentObject);
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