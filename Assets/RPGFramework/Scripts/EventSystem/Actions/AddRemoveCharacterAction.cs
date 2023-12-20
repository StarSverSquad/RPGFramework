using System.Collections;
using UnityEngine;

public class AddRemoveCharacterAction : GraphActionBase
{
    public bool isAdd;

    public RPGCharacter character;

    public AddRemoveCharacterAction() : base("AddRemoveCharacter")
    {
        character = null;
        isAdd = true;
    }

    public override IEnumerator ActionCoroutine()
    {
        if (isAdd)
            GameManager.Instance.characterManager.AddCharacter(character);
        else
            GameManager.Instance.characterManager.RemoveCharacter(character);

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