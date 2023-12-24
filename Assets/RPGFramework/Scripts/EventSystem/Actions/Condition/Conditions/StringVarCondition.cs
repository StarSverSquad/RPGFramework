using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringVarCondition : ConditionBase
{
    public string Var;

    public string Value;

    public StringVarCondition()
    {
        Var = string.Empty;
        Value = string.Empty;
    }

    public override bool Invoke()
    {
        if (!GameManager.Instance.GameData.StringValues.HaveKey(Var))
        {
            Debug.LogWarning($"BOOL_VAR_CONDITION: Переменная {Var} не найдена");

            return false;
        }

        return GameManager.Instance.GameData.StringValues[Var] == Value;
    }

    public override string GetLabel()
    {
        return "По строковой переменной";
    }
}
