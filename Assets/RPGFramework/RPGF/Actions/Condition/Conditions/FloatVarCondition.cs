using RPGF;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatVarCondition : ConditionBase
{
    public string Var;

    public float Value;

    public ConditionOperation Operation;

    public FloatVarCondition()
    {
        Var = string.Empty;
        Value = 0;
        Operation = ConditionOperation.Equals;
    }

    public override bool Invoke()
    {
        if (!GlobalManager.Instance.GameData.FloatValues.HaveKey(Var))
            return false;

        return Operation switch
        {
            ConditionOperation.Equals => GlobalManager.Instance.GameData.FloatValues[Var] == Value,
            ConditionOperation.NotEquals => GlobalManager.Instance.GameData.FloatValues[Var] != Value,
            ConditionOperation.More => GlobalManager.Instance.GameData.FloatValues[Var] > Value,
            ConditionOperation.Less => GlobalManager.Instance.GameData.FloatValues[Var] < Value,
            ConditionOperation.MoreOrEquals => GlobalManager.Instance.GameData.FloatValues[Var] >= Value,
            ConditionOperation.LessOrEquals => GlobalManager.Instance.GameData.FloatValues[Var] <= Value,
            _ => false,
        };
    }

    public override string GetLabel()
    {
        return "По дробной переменной";
    }
}
