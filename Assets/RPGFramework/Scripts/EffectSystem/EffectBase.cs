using System;
using System.Collections;

[Serializable]
public abstract class EffectBase
{
    /// <summary>
    /// Значения от которое зависит эффекстивность эффекта
    /// </summary>
    public float Factor { get; set; } = 1f;

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
