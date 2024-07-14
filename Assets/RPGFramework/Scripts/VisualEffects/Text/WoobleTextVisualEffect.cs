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
        Mesh mesh;
        Vector3[] vertices;

        while (true)
        {
            textMesh.ForceMeshUpdate();
            mesh = textMesh.mesh;
            vertices = mesh.vertices;

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 offset = Wobble(Time.time + i);

                vertices[i] = vertices[i] + offset;
            }

            mesh.vertices = vertices;
            textMesh.canvasRenderer.SetMesh(mesh);

            yield return null;
        }
    }

    private Vector2 Wobble(float time)
    {
        return new Vector2(Mathf.Sin(time * 3.3f), Mathf.Cos(time * 2.5f));
    }
}
