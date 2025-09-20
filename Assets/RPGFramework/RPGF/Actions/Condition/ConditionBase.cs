using RPGF;
using System;

[Serializable]
public abstract class ConditionBase
{
    protected GameManager Game => GameManager.Instance;
    protected LocalManager Local => LocalManager.Instance;

    public abstract bool Invoke();

    public virtual string GetLabel() => "NAME";
}