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

        while (true)
        {
            transformText.ResetMesh();

            for (int i = 0; i < transformText.CharactersCount; i++)
            {
                float randX = Random.Range(-3f, 3f);
                float randY = Random.Range(-3f, 3f);

                transformText.SetCharacterRelativePosition(i, new Vector2(randX, randY));
            }

            transformText.UpdateMesh();

            yield return null;
        }
    }
}