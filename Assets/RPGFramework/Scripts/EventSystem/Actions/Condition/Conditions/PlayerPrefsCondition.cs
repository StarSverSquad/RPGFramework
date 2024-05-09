using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsCondition : ConditionBase
{
    public string Key;

    public int Value;

    public ConditionOperation Operation;

    public PlayerPrefsCondition()
    {
        Key = string.Empty;
        Value = 0;
        Operation = ConditionOperation.Equals;
    }

    public override bool Invoke()
    {
        if (!PlayerPrefs.HasKey(Key))
            return false;

        return PlayerPrefs.GetInt(Key) == Value;
    }

    public override string GetLabel()
    {
        return "По PlayerPrefs";
    }
}
