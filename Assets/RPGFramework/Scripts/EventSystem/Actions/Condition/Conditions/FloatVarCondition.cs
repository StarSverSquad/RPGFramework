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
        if (!GameManager.Instance.GameData.FloatValues.HaveKey(Var))
            return false;

        return Operation switch
        {
            ConditionOperation.Equals => GameManager.Instance.GameData.FloatValues[Var] == Value,
            ConditionOperation.NotEquals => GameManager.Instance.GameData.FloatValues[Var] != Value,
            ConditionOperation.More => GameManager.Instance.GameData.FloatValues[Var] > Value,
            ConditionOperation.Less => GameManager.Instance.GameData.FloatValues[Var] < Value,
            ConditionOperation.MoreOrEquals => GameManager.Instance.GameData.FloatValues[Var] >= Value,
            ConditionOperation.LessOrEquals => GameManager.Instance.GameData.FloatValues[Var] <= Value,
            _ => false,
        };
    }

    public override string GetLabel()
    {
        return "По дробной переменной";
    }
}
