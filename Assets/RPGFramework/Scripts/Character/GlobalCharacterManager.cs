using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlobalCharacterManager : MonoBehaviour, IManagerInitialize
{
    public event Action OnCharaterListChanged;

    [SerializeField] // DEBUG
    private List<RPGCharacter> characters = new List<RPGCharacter>();
    public RPGCharacter[] Characters => characters.ToArray();

    [SerializeField] // DEBUG
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
        return registredCharacters.Any(i => i.Name == character.Name);
    }

    public RPGCharacter GetRegisteredCharacter(RPGCharacter character)
    {
        if (!CharacterIsRegisted(character))
            RegisterCharacter(character);

        return registredCharacters.FirstOrDefault(i => i.Name == character.Name);
    }
}
