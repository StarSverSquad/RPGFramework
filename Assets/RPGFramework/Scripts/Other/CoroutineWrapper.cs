using System;
using System.Collections;
using UnityEngine;

public class CoroutineWrapper
{
    public delegate IEnumerator CoroutineFunc();

    private CoroutineFunc func;
    private MonoBehaviour handler;

    private Coroutine coroutine;

    public bool IsWorking => coroutine != null;

    public CoroutineWrapper(CoroutineFunc func, MonoBehaviour handler)
    {
        this.func = func;
        this.handler = handler;

        coroutine = null;
    }

    public void Start()
    {
        Stop();

        coroutine = handler.StartCoroutine(MainCoroutine());
    }

    public void Stop()
    {
        if (IsWorking)
            handler.StopCoroutine(coroutine);
    }

    private IEnumerator MainCoroutine()
    {
        yield return func.Invoke();

        coroutine = null;
    }
}