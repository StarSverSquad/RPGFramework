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
        if (!GameManager.Instance.gameData.BoolValues.HaveKey(Var))
        {
            Debug.LogWarning($"BOOL_VAR_CONDITION: ���������� {Var} �� �������");

            return false;
        }

        return GameManager.Instance.gameData.BoolValues[Var] == Value;
    }
}