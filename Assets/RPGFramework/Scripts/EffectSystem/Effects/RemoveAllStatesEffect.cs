using System.Collections;

public class RemoveAllStatesEffect : EffectBase
{
    public override IEnumerator Invoke(RPGEntity user, RPGEntity target)
    {
        target.RemoveAllStates();

        yield break;
    }

    public override string GetName()
    {
        return "Убрать все состояния";
    }
}