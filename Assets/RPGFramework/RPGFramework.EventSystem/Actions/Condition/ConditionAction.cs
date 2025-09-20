using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionAction : GraphActionBase
{
    [SerializeReference]
    public List<ConditionBase> Conditions;

    public ConditionAction() : base("ConditionAction")
    {
        Conditions = new List<ConditionBase>();
    }

    public override IEnumerator ActionCoroutine()
    {
        bool isRight = true;

        foreach (ConditionBase c in Conditions)
        {
            if (!c.Invoke())
            {
                isRight = false;
                break;
            }
        }

        nextIndex = isRight ? 0 : 1;

        yield break;
    }

    public override string GetHeader()
    {
        return "Условие";
    }
}

/// <summary>
/// Операции для события сравнения
/// </summary>
public enum ConditionOperation
{
    Equals, NotEquals, More, Less, MoreOrEquals, LessOrEquals
}