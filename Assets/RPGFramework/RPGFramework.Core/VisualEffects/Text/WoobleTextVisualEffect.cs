using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WoobleTextVisualEffect : TextVisualEffectBase
{
    public WoobleTextVisualEffect(TextMeshProUGUI textMesh, MonoBehaviour listener) : base(textMesh, listener)
    {
    }

    protected override IEnumerator EffectCoroutine(TextMeshProUGUI textMesh)
    {
        var transformMesh = new TransformTextMeshService(textMesh);

        while (true)
        {
            transformMesh.ResetMesh();

            for (int i = 0; i < transformMesh.VerticesCount; i++)
            {
                Vector3 offset = Wobble(Time.time + i);

                transformMesh.SetVertexPosition(i, offset);
            }

            transformMesh.UpdateMesh();

            yield return null;
        }
    }

    private Vector2 Wobble(float time)
    {
        return new Vector2(Mathf.Sin(time * 3.3f), Mathf.Cos(time * 2.5f));
    }

    public override string GetTittle()
    {
        return "Плавающий текст";
    }
}
