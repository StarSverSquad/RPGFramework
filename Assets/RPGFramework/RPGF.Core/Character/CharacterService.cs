using System;
using System.Collections.Generic;
using System.Linq;
using RPGF.Domain.DI;
using RPGF.RPG;

namespace RPGF.Core.Character
{
    public class CharacterService : IDisposable, ISupportDI
    {
        public event Action OnCharaterListChanged;

        private readonly List<RPGCharacter> characters;
        public RPGCharacter[] Characters => characters.ToArray();

        private readonly List<RPGCharacter> registeredCharacters;
        public RPGCharacter[] RegistredCharacters => characters.ToArray();

        public CharacterService()
        {
            characters = new List<RPGCharacter>();
            registeredCharacters = new List<RPGCharacter>();
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

            registeredCharacters.Add(character.Clone() as RPGCharacter);
        }

        public bool CharacterIsRegisted(RPGCharacter character)
        {
            return registeredCharacters.Any(i => i.Tag == character.Tag);
        }

        public RPGCharacter GetRegisteredCharacter(RPGCharacter character)
        {
            if (!CharacterIsRegisted(character))
                RegisterCharacter(character);

            return registeredCharacters.FirstOrDefault(i => i.Tag == character.Tag);
        }

        public void Dispose()
        {
            registeredCharacters.Clear();
            characters.Clear();
        }
    }
}