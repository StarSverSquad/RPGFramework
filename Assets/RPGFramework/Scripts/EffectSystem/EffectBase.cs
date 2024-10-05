using System;
using System.Collections;

[Serializable]
public abstract class EffectBase
{
    public virtual IEnumerator Invoke(BattleEntityInfo user, BattleEntityInfo target)
    {
        yield break;
    }

    public abstract string GetName();

    public override string ToString()
    {
        return GetName();
    }
}
