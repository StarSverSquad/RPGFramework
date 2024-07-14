using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FallingText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMesh;

    [SerializeField]
    private AnimationCurve fallingCurve;

    public bool DeleteOnEnd = true;
    public float DeletionDelay = 1f;

    public float FallDelay = 0.1f;
    public float FallSpeed = 1f;

    public float Distance = 20f;

    private Coroutine animationCoroutine;
    public bool IsAnimate => animationCoroutine != null;

    private Color32 startColor;
    private Color32 endColor;

    private List<int> indexes;

    public void Invoke(string text, Color32 startColor, Color32 endColor)
    {
        if (IsAnimate)
            return;

        textMesh.text = text;

        this.startColor = startColor;
        this.startColor.a = 0;

        this.endColor = endColor;
        this.endColor.a = 255;

        indexes = new List<int>();

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
            transformText.SetCharacterPosition(i, new Vector2(0, fallingCurve.Evaluate(0) * Distance));
        }

        for (int i = 0; i < transformText.CharactersCount; i++)
        {
            StartCoroutine(CharacterFall(transformText, i));

            yield return new WaitForSeconds(FallDelay);
        }

        yield return new WaitWhile(() => indexes.Count > 0);

        animationCoroutine = null;

        if (DeleteOnEnd)
        {
            yield return new WaitForSeconds(DeletionDelay);

            Destroy(gameObject);
        }
    }

    private IEnumerator CharacterFall(TransformTextMeshService transformText, int index)
    {
        indexes.Add(index);

        float time = 0;

        while (time < 1)
        {
            Color32 current = Color.Lerp(startColor, endColor, time);

            transformText.SetCharacterColor(index, current);
            transformText.SetCharacterPosition(index, new Vector2(0, fallingCurve.Evaluate(time) * Distance));

            yield return new WaitForFixedUpdate();

            time += Time.fixedDeltaTime * FallSpeed;
        }

        indexes.Remove(index);
    }
}
