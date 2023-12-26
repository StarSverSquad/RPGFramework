using System.Collections;
using UnityEngine;

public abstract class VisualDeathEffectBase : MonoBehaviour
{
    public bool IsAnimating => deathCoroutine != null;

    protected Coroutine deathCoroutine = null;

    public virtual void Invoke()
    {
        if (!IsAnimating)
        {
            deathCoroutine = StartCoroutine(DeathCoroutine());
        }
    }

    protected void EndCoroutinePart() => deathCoroutine = null;

    public abstract void Cleanup();

    protected abstract IEnumerator DeathCoroutine();
}