using RPGF;

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
        if (!GlobalManager.Instance.GameData.StringValues.HaveKey(Var))
            return false;

        return GlobalManager.Instance.GameData.StringValues[Var] == Value;
    }

    public override string GetLabel()
    {
        return "По строковой переменной";
    }
}
