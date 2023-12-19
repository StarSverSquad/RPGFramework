using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEventBase : ScriptableObject
{

    private MonoBehaviour listener;
    public MonoBehaviour Listener => listener;

    [SerializeReference]
    public List<GameActionBase> Actions = new List<GameActionBase>();

    public bool IsPlaying => coroutine != null && listener != null;

    private Coroutine coroutine;

    public virtual void Invoke(MonoBehaviour listener)
    {
        if (!IsPlaying)
        {
            this.listener = listener;
            coroutine = this.listener.StartCoroutine(EventCoroutine());
        }
    }

    public virtual void Break()
    {
        if (IsPlaying)
        {
            listener.StopCoroutine(coroutine);
            coroutine = null;
            listener = null;
        }
    }

    public virtual void EndEventPart()
    {
        coroutine = null;
        listener = null;
    }

    protected abstract IEnumerator EventCoroutine();
}
