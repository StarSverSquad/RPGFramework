using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GlobalCharacterManager : MonoBehaviour, IManagerInitialize
{
    public event Action OnCharaterListChanged;

    private List<RPGCharacter> CharacterList = new List<RPGCharacter>();

    public RPGCharacter[] characters => CharacterList.ToArray();

    public void AddCharacter(RPGCharacter character, bool force = false)
    {
        if (!force && CharacterList.Contains(character))
            return;

        CharacterList.Add(character);

        character.InitializeEntity();

        OnCharaterListChanged?.Invoke();
    }

    public void RemoveCharacter(RPGCharacter character)
    {
        if (!CharacterList.Contains(character))
            return;

        CharacterList.Remove(character);

        OnCharaterListChanged?.Invoke();
    }

    public void Initialize()
    {
        //CharacterList = GameManager.Instance.CommonConfig.playableCharacters;

        //foreach (var item in CharacterList)
        //    item.InitializeEntity();
    }
}
