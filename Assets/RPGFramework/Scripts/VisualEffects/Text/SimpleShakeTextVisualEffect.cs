using DG.Tweening;
using DG.Tweening.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimpleShakeTextVisualEffect : TextVisualEffectBase
{
    public SimpleShakeTextVisualEffect(TextMeshProUGUI textMesh, MonoBehaviour listener) : base(textMesh, listener)
    {
    }

    protected override IEnumerator EffectCoroutine(TextMeshProUGUI textMesh)
    {
        TransformTextMeshService transformText = new TransformTextMeshService(textMesh);

        while (true)
        {
            transformText.ResetMesh();

            for (int i = 0; i < transformText.CharactersCount; i++)
            {
                float randX = UnityEngine.Random.Range(-3f, 3f);
                float randY = UnityEngine.Random.Range(-3f, 3f);

                transformText.SetCharacterRelativePosition(i, new Vector2(randX, randY));
            }

            yield return new WaitForSeconds(0.03f);
        }
    }
}