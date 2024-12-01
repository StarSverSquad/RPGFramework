using System.Collections;

public class ChangeConcentrationEffect : EffectBase
{
    public int AddConcentration;

    public override IEnumerator Invoke(RPGEntity user, RPGEntity target)
    {
        BattleManager.Utility.AddConcetration(AddConcentration);

        yield break;
    }

    public override string GetName()
    {
        return "Изменить концентрацию";
    }
}