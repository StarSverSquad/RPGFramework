using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlobalCharacterManager : MonoBehaviour, IManagerInitialize, IDisposable
{
    public event Action OnCharaterListChanged;

    private List<RPGCharacter> characters = new List<RPGCharacter>();
    public RPGCharacter[] Characters => characters.ToArray();

    private List<RPGCharacter> registredCharacters = new List<RPGCharacter>();
    public RPGCharacter[] RegistredCharacters => characters.ToArray();

    public void Initialize()
    {

    }

    public void AddCharacter(RPGCharacter character, bool initialize = true)
    {
        RPGCharacter trueCharacter = GetRegisteredCharacter(character);

        if (characters.Contains(trueCharacter) || trueCharacter == null)
            return;

        characters.Add(trueCharacter);

        if (initialize)
            trueCharacter.InitializeEntity();

        OnCharaterListChanged?.Invoke();
    }

    public void RemoveCharacter(RPGCharacter character)
    {
        RPGCharacter trueCharacter = GetRegisteredCharacter(character);

        if (trueCharacter == null || !characters.Contains(trueCharacter))
            return;

        characters.Remove(trueCharacter);

        OnCharaterListChanged?.Invoke();
    }

    public void RegisterCharacter(RPGCharacter character)
    {
        if (CharacterIsRegisted(character))
            return;

        registredCharacters.Add(Instantiate(character));
    }

    public bool CharacterIsRegisted(RPGCharacter character)
    {
        return registredCharacters.Any(i => i.Tag == character.Tag);
    }

    public RPGCharacter GetRegisteredCharacter(RPGCharacter character)
    {
        if (!CharacterIsRegisted(character))
            RegisterCharacter(character);

        return registredCharacters.FirstOrDefault(i => i.Tag == character.Tag);
    }

    public void Dispose()
    {
        registredCharacters.Clear();
        characters.Clear();
    }
}
