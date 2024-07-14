using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class SimpleShakeTextVisualEffect : TextVisualEffectBase
{
    public SimpleShakeTextVisualEffect(TextMeshProUGUI textMesh, MonoBehaviour listener) : base(textMesh, listener)
    {
    }

    protected override IEnumerator EffectCoroutine(TextMeshProUGUI textMesh)
    {
        var transformText = new TransformTextMeshService(textMesh);

        DateTime cur, prev = DateTime.Now;

        yield return null;

        transformText.ResetMesh();

        while (true)
        {
            cur = DateTime.Now;

            if ((cur - prev).TotalMilliseconds > 40)
            {
                transformText.ResetMesh();

                for (int i = 0; i < transformText.CharactersCount; i++)
                {
                    float randX = UnityEngine.Random.Range(-3f, 3f);
                    float randY = UnityEngine.Random.Range(-3f, 3f);

                    transformText.SetCharacterPosition(i, new Vector2(randX, randY));
                }

                prev = cur;
            }
            else
                transformText.PartialResetMesh();

            transformText.UpdateMesh();

            yield return null;
        }
    }

    public override string GetTittle()
    {
        return "Тряска текста";
    }
}