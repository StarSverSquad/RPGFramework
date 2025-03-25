using System;
using System.Collections;
using UnityEngine;

public abstract class VisualEffectBase : RPGFrameworkBehaviour, IDisposable
{
    public bool IsPlaying => effectCoroutine != null;

    private Coroutine effectCoroutine;

    public event Action OnPlay;
    public event Action OnEndPlay;
    public event Action OnStoped;

    public void Play()
    {
        if (IsPlaying)
            return;

        effectCoroutine = StartCoroutine(MainCoroutine());
    }
    public void Stop()
    {
        StopEffectCoroutine();

        OnStoped?.Invoke();
    }

    protected abstract IEnumerator EffectCoroutine();

    private IEnumerator MainCoroutine()
    {
        OnPlay?.Invoke();

        yield return EffectCoroutine();

        OnEndPlay?.Invoke();

        effectCoroutine = null;
    }

    public virtual void Dispose()
    {
        StopEffectCoroutine();
    }

    private void StopEffectCoroutine()
    {
        if (effectCoroutine != null)
        {
            StopCoroutine(effectCoroutine);
            effectCoroutine = null;
        }
    }

    private void OnDestroy()
    {
        Dispose();
    }
}