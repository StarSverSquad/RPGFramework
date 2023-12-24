using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCondition : ConditionBase
{
    public int Value;

    public ConditionOperation Operation;

    public MoneyCondition()
    {
        Value = 0;
        Operation = ConditionOperation.Equals;
    }

    public override bool Invoke()
    {
        return Operation switch
        {
            ConditionOperation.Equals => GameManager.Instance.GameData.Money == Value,
            ConditionOperation.NotEquals => GameManager.Instance.GameData.Money != Value,
            ConditionOperation.More => GameManager.Instance.GameData.Money > Value,
            ConditionOperation.Less => GameManager.Instance.GameData.Money < Value,
            ConditionOperation.MoreOrEquals => GameManager.Instance.GameData.Money >= Value,
            ConditionOperation.LessOrEquals => GameManager.Instance.GameData.Money <= Value,
            _ => false,
        };
    }

    public override string GetLabel()
    {
        return "По деньгам";
    }
}
