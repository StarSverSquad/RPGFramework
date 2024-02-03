using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolVarCondition : ConditionBase
{
    public string Var;

    public bool Value;

    public BoolVarCondition()
    {
        Var = string.Empty;
        Value = false;
    }

    public override bool Invoke()
    {
        if (!GameManager.Instance.GameData.BoolValues.HaveKey(Var))
            return false;

        return GameManager.Instance.GameData.BoolValues[Var] == Value;
    }

    public override string GetLabel()
    {
        return "По переключателю";
    }
}
