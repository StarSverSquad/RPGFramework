using RPGF.Character;
using System.Collections;

public class AddRemoveCharacterAction : GraphActionBase
{
    public bool isAdd;
    public bool updateModels;

    public RPGCharacter character;

    public PlayableCharacterModelController existentObject;

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
            GameManager.Instance.Character.AddCharacter(character);
        else
            GameManager.Instance.Character.RemoveCharacter(character);

        if (updateModels)
            LocalManager.Instance.Character.RebuildModels();
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

    public override object Clone()
    {
        return new AddRemoveCharacterAction()
        {
            isAdd = isAdd,
            character = character,
            updateModels = updateModels,
            existentObject = existentObject
        };
    }
}