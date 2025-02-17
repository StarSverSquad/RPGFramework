using System;
using System.Collections;
using UnityEngine;

public abstract class MinigameBase : RPGFrameworkBehaviour
{
    private Coroutine coroutine = null;
    public bool IsRuning => coroutine != null;

    protected float winFactor = 1f;
    public float WinFactor => winFactor;

    public event Action OnStart;
    public event Action OnEnd;

    public void Invoke()
    {
        if (!IsRuning)
            coroutine = StartCoroutine(BaseCoroutine());
    }

    private IEnumerator BaseCoroutine()
    {
        OnStart?.Invoke();

        OnMinigameStart();

        yield return StartCoroutine(Minigame());

        coroutine = null;

        OnMinigameEnd();

        OnEnd?.Invoke();
    }

    protected virtual void OnMinigameStart() { }
    protected virtual void OnMinigameEnd() { }

    protected void SetWinFactor(float value) => winFactor = value;

    protected abstract IEnumerator Minigame();
}
