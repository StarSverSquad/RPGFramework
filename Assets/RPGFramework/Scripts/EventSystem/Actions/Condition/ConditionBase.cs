using System;
using System.Collections;
using UnityEngine;

[Serializable]
public abstract class ConditionBase
{
    public abstract bool Invoke();

    public virtual string GetLabel() => "NAME";
}