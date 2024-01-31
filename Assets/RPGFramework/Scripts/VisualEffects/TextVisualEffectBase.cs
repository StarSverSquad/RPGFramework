using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class TextVisualEffectBase : MonoBehaviour
{
    private Coroutine effectCoroutine;
    public bool IsPlaying => effectCoroutine != null;

    public void StartEffect(TextMeshProUGUI textMesh)
    {
        if (IsPlaying)
            return;

        effectCoroutine = StartCoroutine(EffectCoroutine(textMesh));
    }

    public void StopEffect()
    {
        if (!IsPlaying)
            return;

        StopCoroutine(effectCoroutine);

        effectCoroutine = null;
    }

    protected abstract IEnumerator EffectCoroutine(TextMeshProUGUI textMesh);
}
