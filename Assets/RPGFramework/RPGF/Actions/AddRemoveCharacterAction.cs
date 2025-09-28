using RPGF;
using RPGF.Core.Character;
using RPGF.EventSystem;
using RPGF.RPG;
using System.Collections;

public class AddRemoveCharacterAction : ActionBase
{
    public bool isAdd;
    public bool updateModels;

    public RPGCharacter character;

    public PlayableCharacterModelController existentObject;

    public AddRemoveCharacterAction() : base()
    {
        character = null;
        existentObject = null;
        isAdd = true;
        updateModels = true;
    }

    public override IEnumerator ActionCoroutine()
    {
        if (isAdd)
            GlobalManager.Instance.Character.AddCharacter(character);
        else
            GlobalManager.Instance.Character.RemoveCharacter(character);

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