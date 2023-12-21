using System;
using System.Collections;

[Serializable]
public abstract class EffectBase
{
    public virtual IEnumerator BattleInvoke()
    {
        yield break;
    }

    public virtual IEnumerator ExplorerInvoke()
    {
        yield break;
    }

    public abstract string GetName();

    public override string ToString()
    {
        return GetName();
    }
}
