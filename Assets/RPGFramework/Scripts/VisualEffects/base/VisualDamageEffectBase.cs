using System.Collections;
using UnityEngine;

public abstract class VisualDamageEffectBase : MonoBehaviour
{
    public bool IsAnimating => damageCoroutine != null;

    protected Coroutine damageCoroutine = null;

    public virtual void Invoke()
    {
        if (!IsAnimating)
        {
            damageCoroutine = StartCoroutine(DamageCoroutine());
        }
    }

    protected void EndCoroutinePart() => damageCoroutine = null;

    public abstract void Cleanup();

    protected abstract IEnumerator DamageCoroutine();
}