using RPGF.Core.Character;
using RPGF.Domain.DI;
using RPGF.EventSystem;
using RPGF.RPG;
using System;
using System.Collections;

namespace RPGF.Actions
{
    [Serializable]
    public class AddRemoveCharacterAction : ActionBase
    {
        [Inject]
        private readonly CharacterManager _character;
        [Inject]
        private readonly CharacterService _characterService;

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
                _characterService.AddCharacter(character);
            else
                _characterService.RemoveCharacter(character);

            if (updateModels)
            {
                _character.RebuildModels();
            }
            else if (existentObject != null)
            {
                if (isAdd)
                    _character.AddModel(existentObject);
                else
                    _character.RemoveModel(existentObject);
            }

            yield break;
        }

        public override ActionBase Clone()
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
}