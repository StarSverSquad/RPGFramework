using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class GameActionBase
{
    public readonly string Name;

    public bool Launched => coroutine != null;

    protected GameEventBase gameEvent;
    protected Coroutine coroutine;

    public GameEventBase GameEvent
    {
        get { return gameEvent; }
        set { gameEvent = value; }
    }

    public GameActionBase(string name)
    {
        Name = name;
    }
    public GameActionBase()
    {
        Name = "Action";
    }

    public virtual Coroutine Launch(GameEventBase gameEvent)
    {
        if (coroutine != null)
            this.gameEvent.Listener.StopCoroutine(coroutine);

        this.gameEvent = gameEvent;

        coroutine = this.gameEvent.Listener.StartCoroutine(ActionCoroutine());

        return coroutine;
    }

    public abstract IEnumerator ActionCoroutine();

    public virtual string GetInfo()
    {
        return string.Empty;
    }
    public virtual string GetHeader()
    {
        return Name;
    }

    public override string ToString()
    {
        return Name;
    }
}
