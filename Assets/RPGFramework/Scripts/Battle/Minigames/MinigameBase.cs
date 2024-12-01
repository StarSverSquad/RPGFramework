using System.Collections;
using UnityEngine;

public abstract class MinigameBase : RPGFrameworkBehaviour
{
    [SerializeField]
    protected GameObject Content;

    private Coroutine coroutine = null;
    public bool IsRuning => coroutine != null;

    protected float winFactor = 1f;
    public float WinFactor => winFactor;

    public void Invoke()
    {
        if (!IsRuning)
            coroutine = StartCoroutine(BaseCoroutine());
    }

    private IEnumerator BaseCoroutine()
    {
        yield return StartCoroutine(Minigame());

        coroutine = null;
    }

    protected abstract IEnumerator Minigame();
}
