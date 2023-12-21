using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntVarCondition : ConditionBase
{
    public string Var;

    public int Value;

    public ConditionOperation Operation;

    public IntVarCondition()
    {
        Var = string.Empty;
        Value = 0;
        Operation = ConditionOperation.Equals;
    }

    public override bool Invoke()
    {
        if (!GameManager.Instance.gameData.IntValues.HaveKey(Var))
        {
            Debug.LogWarning($"INT_VAR_CONDITION: Переменная {Var} не найдена");

            return false;
        }

        return Operation switch
        {
            ConditionOperation.Equals => GameManager.Instance.gameData.IntValues[Var] == Value,
            ConditionOperation.NotEquals => GameManager.Instance.gameData.IntValues[Var] != Value,
            ConditionOperation.More => GameManager.Instance.gameData.IntValues[Var] > Value,
            ConditionOperation.Less => GameManager.Instance.gameData.IntValues[Var] < Value,
            ConditionOperation.MoreOrEquals => GameManager.Instance.gameData.IntValues[Var] >= Value,
            ConditionOperation.LessOrEquals => GameManager.Instance.gameData.IntValues[Var] <= Value,
            _ => false,
        };
    }

    public override string GetLabel()
    {
        return "По целочисленной переменной";
    }
}
