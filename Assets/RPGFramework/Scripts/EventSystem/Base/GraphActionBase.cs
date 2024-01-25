using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class GraphActionBase : GameActionBase, ICloneable
{
    [SerializeReference]
    public List<GraphActionBase> NextActions;

    protected int nextIndex;

    public GraphActionBase Next
    {
        get
        {
            if (nextIndex < 0 || nextIndex > NextActions.Count - 1)
                return null;

            return NextActions[nextIndex];
        }
    }

    public GraphActionBase(string name) : base(name)
    {
        NextActions = new List<GraphActionBase>();

        nextIndex = 0;
    }
    public GraphActionBase() : base("GraphAction")
    {
        NextActions = new List<GraphActionBase>();

        nextIndex = 0;
    }

    public virtual object Clone() => new();
}
