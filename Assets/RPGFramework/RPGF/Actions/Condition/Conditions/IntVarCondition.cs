using RPGF;

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
        if (!GlobalManager.Instance.GameData.IntValues.HaveKey(Var))
            return false;

        return Operation switch
        {
            ConditionOperation.Equals => GlobalManager.Instance.GameData.IntValues[Var] == Value,
            ConditionOperation.NotEquals => GlobalManager.Instance.GameData.IntValues[Var] != Value,
            ConditionOperation.More => GlobalManager.Instance.GameData.IntValues[Var] > Value,
            ConditionOperation.Less => GlobalManager.Instance.GameData.IntValues[Var] < Value,
            ConditionOperation.MoreOrEquals => GlobalManager.Instance.GameData.IntValues[Var] >= Value,
            ConditionOperation.LessOrEquals => GlobalManager.Instance.GameData.IntValues[Var] <= Value,
            _ => false,
        };
    }

    public override string GetLabel()
    {
        return "По целочисленной переменной";
    }
}
