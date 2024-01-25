using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public abstract class EffectBase
{
    protected Dictionary<string, object> effectInfo = new Dictionary<string, object>();

    public object this[string key]
    {
        get => GetInfo(key);
    }

    protected void AddInfo(string key, object value)
    {
        if (!effectInfo.ContainsKey(key))
            effectInfo.Add(key, value);
        else
            effectInfo[key] = value;
    }

    public object GetInfo(string key)
    {
        if (effectInfo.ContainsKey(key))
            return effectInfo[key];
        return null;
    }

    public bool InfoIsExists(string key) => effectInfo.ContainsKey(key);

    public virtual IEnumerator BattleInvoke(BattleEntityInfo user, BattleEntityInfo target)
    {
        yield break;
    }

    public virtual IEnumerator ExplorerInvoke(RPGEntity user, RPGEntity target)
    {
        yield break;
    }

    public abstract string GetName();

    public override string ToString()
    {
        return GetName();
    }
}
