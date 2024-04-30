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
        while (true)
        {
            textMesh.ForceMeshUpdate();

            TMP_TextInfo textInfo = textMesh.textInfo;

            if (textInfo == null)
            {
                yield return new WaitForFixedUpdate();
                continue;
            }

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                var characterInfo = textInfo.characterInfo[i];

                if (!characterInfo.isVisible)
                {
                    yield return new WaitForFixedUpdate();
                    continue;
                }

                var vertexes = textInfo.meshInfo[characterInfo.materialReferenceIndex].vertices;


                float randX = UnityEngine.Random.Range(-3f, 3f);
                float randY = UnityEngine.Random.Range(-3f, 3f);

                for (int j = 0; j < 4; j++)
                {
                    vertexes[characterInfo.vertexIndex + j] = new Vector3(
                        vertexes[characterInfo.vertexIndex + j].x + randX,
                        vertexes[characterInfo.vertexIndex + j].y + randY,
                        vertexes[characterInfo.vertexIndex + j].z);
                }
            }

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                TMP_MeshInfo meshInfo = textInfo.meshInfo[i];

                meshInfo.mesh.vertices = meshInfo.vertices;

                textMesh.UpdateGeometry(meshInfo.mesh, i);
            }

            yield return new WaitForSeconds(0.03f);
        }
    }
}