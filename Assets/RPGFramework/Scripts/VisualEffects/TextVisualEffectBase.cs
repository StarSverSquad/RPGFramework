using System.Collections;
using TMPro;
using UnityEngine;

public abstract class TextVisualEffectBase
{
    private Coroutine effectCoroutine;
    public bool IsPlaying => effectCoroutine != null;

    public int StartLetter { get; set; }
    public int EndLetter { get; set; }

    protected TextMeshProUGUI TextMesh;
    protected MonoBehaviour Listener;

    public TextVisualEffectBase(TextMeshProUGUI textMesh, MonoBehaviour listener)
    {
        TextMesh = textMesh;
        Listener = listener;
    }

    public void StartEffect()
    {
        if (IsPlaying)
            return;

        effectCoroutine = Listener.StartCoroutine(EffectCoroutine(TextMesh));

        OnStartEffect();
    }

    public void StopEffect()
    {
        if (!IsPlaying)
            return;

        Listener.StopCoroutine(effectCoroutine);

        effectCoroutine = null;

        OnEndEffect();
    }

    protected abstract IEnumerator EffectCoroutine(TextMeshProUGUI textMesh);

    public virtual void OnStartEffect() { }

    public virtual void OnEndEffect() { }

    public virtual string GetTittle() => GetType().Name;
}
