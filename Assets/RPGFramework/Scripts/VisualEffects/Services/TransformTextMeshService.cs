using System.Linq;
using TMPro;
using UnityEngine;

public class TransformTextMeshService
{
    private TextMeshProUGUI textMesh;
    private TMP_TextInfo textInfo => textMesh.textInfo;

    private Color[] colors;
    private Vector3[] vertices;
    private Mesh mesh;

    private TMP_CharacterInfo[] visibles;
    // Даааа, я три года думал что делать, по итогу это свойство высчитывалось по пять раз
    public TMP_CharacterInfo[] Visibles => visibles;

    public int CharactersCount => Visibles.Length;

    public TransformTextMeshService(TextMeshProUGUI textMesh)
    {
        this.textMesh = textMesh;

        visibles = textInfo.characterInfo.Where(c => c.isVisible).ToArray();

        ResetMesh();
    }

    public void SetCharacterRelativePosition(int index, Vector3 position)
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

    public void SetCharacterColor(int index, Color32 color)
    {
        if (textInfo == null || Visibles.Length == 0)
            return;

        TMP_CharacterInfo characterInfo = Visibles[index];

        for (int i = 0; i < 4;i ++)
        {
            colors[characterInfo.vertexIndex + i] = color;
        }

        UpdateMesh();
    }

    public void ResetMesh()
    {
        textMesh.ForceMeshUpdate();
 
        visibles = textInfo.characterInfo.Where(c => c.isVisible).ToArray();

        mesh = textMesh.mesh;
        vertices = mesh.vertices;
        colors = mesh.colors;
    }

    public void UpdateMesh()
    {
        mesh.vertices = vertices;

        textMesh.canvasRenderer.SetMesh(mesh);
    }
}