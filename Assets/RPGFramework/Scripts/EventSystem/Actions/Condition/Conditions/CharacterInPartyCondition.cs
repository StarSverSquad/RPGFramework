using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterInPartyCondition : ConditionBase
{
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

        return GameManager.Instance.Character.Characters.Any(i => i.Tag == Value.Tag);
    }

    public override string GetLabel()
    {
        return "Персонаж в команде";
    }
}
