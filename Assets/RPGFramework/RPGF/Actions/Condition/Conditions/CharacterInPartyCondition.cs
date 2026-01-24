using RPGF.Core.Character;
using RPGF.Domain.DI;
using RPGF.RPG;
using System.Linq;
using UnityEngine;

namespace RPGF.Actions.Condition
{
    [UseCondition("Персонаж в команде")]
    public class CharacterInPartyCondition : ConditionBase
    {
        [Inject]
        private readonly CharacterService _character;

        public RPGCharacter Value;

        public CharacterInPartyCondition()
        {
            Value = null;
        }

        public override bool Invoke()
        {
            if (Value == null)
            {
                Debug.LogError($"CHARACTER_IN_PARTY_CONDITION: персонаж не указан");

                return false;
            }

            return _character.Characters.Any(i => i.Tag == Value.Tag);
        }
    }
}
