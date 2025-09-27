using RPGF;
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
        if (!GlobalManager.Instance.GameData.BoolValues.HaveKey(Var))
            return !Value;

        return GlobalManager.Instance.GameData.BoolValues[Var] == Value;
    }

    public override string GetLabel()
    {
        return "По переключателю";
    }
}
