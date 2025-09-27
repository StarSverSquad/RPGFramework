using RPGF.Domain.DI;
using RPGF.RPG;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RPGF.Core.Character
{
    public class CharacterService : IDisposable, ISupportDI
    {
        public event Action OnCharaterListChanged;

        private List<RPGCharacter> characters;
        public RPGCharacter[] Characters => characters.ToArray();

        private List<RPGCharacter> registredCharacters;
        public RPGCharacter[] RegistredCharacters => characters.ToArray();

        public CharacterService()
        {
            characters = new List<RPGCharacter>();
            registredCharacters = new List<RPGCharacter>();
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

            registredCharacters.Add(character.Clone() as RPGCharacter);
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
}