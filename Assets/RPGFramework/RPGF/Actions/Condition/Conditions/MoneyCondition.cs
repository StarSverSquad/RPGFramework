using RPGF;

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
            ConditionOperation.Equals => GlobalManager.Instance.GameData.Money == Value,
            ConditionOperation.NotEquals => GlobalManager.Instance.GameData.Money != Value,
            ConditionOperation.More => GlobalManager.Instance.GameData.Money > Value,
            ConditionOperation.Less => GlobalManager.Instance.GameData.Money < Value,
            ConditionOperation.MoreOrEquals => GlobalManager.Instance.GameData.Money >= Value,
            ConditionOperation.LessOrEquals => GlobalManager.Instance.GameData.Money <= Value,
            _ => false,
        };
    }

    public override string GetLabel()
    {
        return "По деньгам";
    }
}
