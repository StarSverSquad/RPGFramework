using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TransformTextMeshService
{
    private TextMeshProUGUI textMesh;

    private List<Vector3[]> charaterVertexes;

    private TMP_CharacterInfo[] GetVisibles
    {
        get => textInfo != null ? textInfo.characterInfo.Where(c => c.isVisible).ToArray() : new TMP_CharacterInfo[0];
    }

    private TMP_TextInfo textInfo => textMesh.textInfo;

    public int CharactersCount => GetVisibles.Length;

    public TransformTextMeshService(TextMeshProUGUI textMesh)
    {
        this.textMesh = textMesh;

        charaterVertexes = new List<Vector3[]>();

        this.textMesh.OnPreRenderText += (txt) => {
            UpdateCharacterVertexes();
        };
    }

    public void SetCharacterRelativePosition(int index, Vector2 position)
    {
        if (textInfo == null || GetVisibles.Length == 0)
            return;

        TMP_CharacterInfo characterInfo = GetVisibles[index];

        var vertexes = textInfo.meshInfo[characterInfo.materialReferenceIndex].vertices;

        Vector3[] defaultVertexes = charaterVertexes[index];

        for (int i = 0; i < 4; i++)
        {
            vertexes[characterInfo.vertexIndex + i] = new Vector3(
                    defaultVertexes[i].x + position.x,
                    defaultVertexes[i].y + position.y,
                    defaultVertexes[i].z);
        }

        UpdateMesh();
    }

    public void SetCharacterColor(int index, Color32 color)
    {
        if (textInfo == null || GetVisibles.Length == 0)
            return;

        TMP_CharacterInfo characterInfo = GetVisibles[index];

        var colors = textInfo.meshInfo[characterInfo.materialReferenceIndex].colors32;

        for (int i = 0; i < 4;i ++)
        {
            colors[characterInfo.vertexIndex + i] = color;
        }

        UpdateMesh();
    }

    public void ResetMesh()
    {
        textMesh.ForceMeshUpdate();

        UpdateCharacterVertexes();
    }

    private void UpdateMesh()
    {
        if (textInfo == null) 
            return;

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            TMP_MeshInfo meshInfo = textInfo.meshInfo[i];

            meshInfo.mesh.vertices = meshInfo.vertices;
            meshInfo.mesh.colors32 = meshInfo.colors32;

            textMesh.UpdateGeometry(meshInfo.mesh, i);
        }
    }

    private void UpdateCharacterVertexes()
    {
        if (textInfo == null)
            return;

        charaterVertexes.Clear();
        foreach (var item in GetVisibles)
        {
            Vector3[] vertexes = new Vector3[4];

            var vertexesRaw = textInfo.meshInfo[item.materialReferenceIndex].vertices;

            vertexes[0] = vertexesRaw[item.vertexIndex];
            vertexes[1] = vertexesRaw[item.vertexIndex + 1];
            vertexes[2] = vertexesRaw[item.vertexIndex + 2];
            vertexes[3] = vertexesRaw[item.vertexIndex + 3];

            charaterVertexes.Add(vertexes);
        }
    }
}