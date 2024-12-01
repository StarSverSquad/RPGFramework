using System;
using System.Collections;

[Serializable]
public abstract class EffectBase
{
    public virtual IEnumerator Invoke(RPGEntity user, RPGEntity target)
    {
        yield break;
    }

    public abstract string GetName();

    public override string ToString()
    {
        return GetName();
    }
}
