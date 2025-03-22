using UnityEngine;

public class FastSaveCondition : ConditionBase
{
    public string Key;

    public int Value;

    public ConditionOperation Operation;

    public FastSaveCondition()
    {
        Key = string.Empty;
        Value = 0;
        Operation = ConditionOperation.Equals;
    }

    public override bool Invoke()
    {
        if (!GameManager.Instance.SaveLoad.HasFastSaveKey(Key))
        {
            Debug.LogWarning($"{Key} переменная не найдена в FastSaves!");
            return false;
        }

        int savedValue = GameManager.Instance.SaveLoad.GetFastSave(Key);

        return Operation switch
        {
            ConditionOperation.Equals => savedValue == Value,
            ConditionOperation.LessOrEquals => savedValue <= Value,
            ConditionOperation.MoreOrEquals => savedValue >= Value,
            ConditionOperation.Less => savedValue < Value,
            ConditionOperation.More => savedValue > Value,
            ConditionOperation.NotEquals => savedValue != Value,
            _ => false
        };
    }

    public override string GetLabel()
    {
        return "По FastSave";
    }
}
