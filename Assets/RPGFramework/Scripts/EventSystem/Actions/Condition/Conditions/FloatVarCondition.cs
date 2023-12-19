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
        if (!GameManager.Instance.gameData.FloatValues.HaveKey(Var))
        {
            Debug.LogWarning($"FLOAT_VAR_CONDITION: Переменная {Var} не найдена");

            return false;
        }

        return Operation switch
        {
            ConditionOperation.Equals => GameManager.Instance.gameData.FloatValues[Var] == Value,
            ConditionOperation.NotEquals => GameManager.Instance.gameData.FloatValues[Var] != Value,
            ConditionOperation.More => GameManager.Instance.gameData.FloatValues[Var] > Value,
            ConditionOperation.Less => GameManager.Instance.gameData.FloatValues[Var] < Value,
            ConditionOperation.MoreOrEquals => GameManager.Instance.gameData.FloatValues[Var] >= Value,
            ConditionOperation.LessOrEquals => GameManager.Instance.gameData.FloatValues[Var] <= Value,
            _ => false,
        };
    }
}
