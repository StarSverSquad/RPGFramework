using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class FallingText : MonoBehaviour
{
    public enum EffectType
    {
        TopDown,
        Up
    }

    [SerializeField]
    private TextMeshProUGUI textMesh;

    [SerializeField]
    private AnimationCurve topDownCurve;

    [SerializeField]
    private AnimationCurve upCurve;

    public bool DeleteOnEnd = true;

    public float DeletionDelay = 1f;
    public float Delay = 0.1f;
    public float Speed = 1f;

    public EffectType Effect = EffectType.TopDown;

    public float Distance = 20f;

    private Coroutine animationCoroutine;
    private Coroutine meshDrawCoroutine;
    public bool IsAnimate => animationCoroutine != null;

    private Color32 startColor;
    private Color32 endColor;

    private int maxCharacters;

    public void Invoke(string text, Color32 startColor, Color32 endColor)
    {
        if (IsAnimate)
            return;

        textMesh.text = text;

        this.startColor = startColor;
        this.startColor.a = 0;

        this.endColor = endColor;
        this.endColor.a = 255;

        maxCharacters = 1;

        animationCoroutine = StartCoroutine(Animation());
    }

    public void Invoke(string text)
    {
        Invoke(text, Color.white, Color.white);
    }

    private IEnumerator Animation()
    {
        TransformTextMeshService transformText = new TransformTextMeshService(textMesh);

        transformText.ResetMesh();

        for (int i = 0; i < transformText.CharactersCount; i++)
        {
            transformText.SetCharacterColor(i, startColor);
            transformText.SetCharacterPosition(
                i,
                new Vector2(0, topDownCurve.Evaluate(0) * Distance)
            );
        }

        transformText.UpdateMesh();

        meshDrawCoroutine = StartCoroutine(MeshDraw(transformText));

        for (int i = 1; i < transformText.CharactersCount; i++)
        {
            yield return new WaitForSeconds(Delay);

            maxCharacters++;
        }

        yield return new WaitWhile(() => meshDrawCoroutine != null);

        animationCoroutine = null;

        if (DeleteOnEnd)
        {
            yield return new WaitForSeconds(DeletionDelay);

            Destroy(gameObject);
        }
    }

    private IEnumerator MeshDraw(TransformTextMeshService transformText)
    {
        List<float> times = new() { 0 };

        while (times.Any(i => i < 1))
        {
            transformText.ResetMesh();

            for (int i = 0; i < maxCharacters; i++)
            {
                if (times.Count - 1 < i)
                    times.Add(0);

                switch (Effect)
                {
                    default:
                    case EffectType.TopDown:
                        TopDownEffect(transformText, i, times[i]);
                        break;
                    case EffectType.Up:
                        UpEffect(transformText, i, times[i]);
                        break;
                }
            }

            for (int i = maxCharacters; i < transformText.CharactersCount; i++)
            {       
                Color32 clr = Color.white; clr.a = 0;

                transformText.SetCharacterColor(i, clr);
                transformText.SetCharacterPosition(i, new Vector2(0, 0));
            }

            transformText.UpdateMesh();

            yield return null;

            for (int i = 0; i < times.Count; i++)
            {
                times[i] += Time.deltaTime * Speed;
            }
        }

        meshDrawCoroutine = null;
    }

    private void TopDownEffect(TransformTextMeshService transformText, int index, float time)
    {
        Color32 current = Color.Lerp(startColor, endColor, time);

        transformText.SetCharacterColor(index, current);
        transformText.SetCharacterPosition(
            index,
            new Vector2(0, topDownCurve.Evaluate(time) * Distance)
        );
    }

    private void UpEffect(TransformTextMeshService transformText, int index, float time)
    {
        Color32 current = Color.Lerp(startColor, endColor, time);

        transformText.SetCharacterColor(index, current);
        transformText.SetCharacterPosition(
            index,
            new Vector2(0, upCurve.Evaluate(time) * Distance)
        );
    }
}
