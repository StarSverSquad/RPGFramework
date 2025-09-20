using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class TransformTextMeshService
{
    private readonly TextMeshProUGUI textMesh;
    private TMP_TextInfo textInfo => textMesh.textInfo;

    private Color[] colors;
    private Vector3[] vertices;
    private Mesh mesh;

    private TMP_CharacterInfo[] visibles;

    // Даааа, я три года думал что делать, по итогу это свойство высчитывалось по пять раз
    public TMP_CharacterInfo[] Visibles => visibles;

    public int CharactersCount => Visibles.Length;
    public int VerticesCount => vertices.Length;
    public int ColorsCount => colors.Length;

    public TransformTextMeshService(TextMeshProUGUI textMesh)
    {
        this.textMesh = textMesh;

        ResetMesh();
    }

    public void SetCharacterPosition(int index, Vector3 position)
    {
        if (textInfo == null || Visibles.Length == 0)
            return;

        TMP_CharacterInfo characterInfo = Visibles[index];

        for (int i = 0; i < 4; i++)
        {
            vertices[characterInfo.vertexIndex + i].x += position.x;
            vertices[characterInfo.vertexIndex + i].y += position.y;
            vertices[characterInfo.vertexIndex + i].z += position.z;
        }
    }

    public void SetVertexPosition(int index, Vector3 position)
    {
        if (textInfo == null)
            return;

        vertices[index].x += position.x;
        vertices[index].y += position.y;
        vertices[index].z += position.z;
    }

    public void SetCharacterVertecesPosition(int index, Func<Vector3[], Vector3[]> vertecesCallback)
    {
        if (textInfo == null || Visibles.Length == 0)
            return;

        TMP_CharacterInfo characterInfo = Visibles[index];

        Vector3[] cur = vertices.Skip(characterInfo.vertexIndex).Take(4).ToArray();
        Vector3[] modified = vertecesCallback(cur);

        for (int i = 0; i < 4; i++)
            vertices[characterInfo.vertexIndex + i] = modified[i];
    }

    public void SetCharacterColor(int index, Color32 color)
    {
        if (textInfo == null || Visibles.Length == 0)
            return;

        TMP_CharacterInfo characterInfo = Visibles[index];

        for (int i = 0; i < 4; i++)
        {
            colors[characterInfo.vertexIndex + i] = color;
        }
    }

    public void ResetMesh()
    {
        textMesh.ForceMeshUpdate();

        visibles = textInfo.characterInfo.Where(c => c.isVisible).ToArray();

        mesh = textMesh.mesh;

        if (mesh == null)
            return;

        vertices = mesh.vertices;
        colors = mesh.colors;
    }

    public void PartialResetMesh()
    {
        textMesh.ForceMeshUpdate();

        visibles = textInfo.characterInfo.Where(c => c.isVisible).ToArray();

        mesh = textMesh.mesh;

        if (mesh.vertices.Length > vertices.Length)
            vertices = vertices.Concat(mesh.vertices.Skip(vertices.Length)).ToArray();

        if (mesh.colors.Length > colors.Length)
            colors = colors.Concat(mesh.colors.Skip(colors.Length)).ToArray();
    }

    public void UpdateMesh()
    {
        mesh.vertices = vertices;
        mesh.colors = colors;

        textMesh.canvasRenderer.SetMesh(mesh);
    }
}
