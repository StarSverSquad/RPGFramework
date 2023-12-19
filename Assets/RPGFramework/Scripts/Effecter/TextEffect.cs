using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;

public class TextEffect : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;

    private void Start()
    {
        StartCoroutine(AnimationCoroutine());
    }

    // Use this for initialization
    private void FixedUpdate()
    {
        
    }

    public void TranslateCharacterPosition(int index, Vector2 offset)
    {
        TMP_TextInfo textInfo = textMeshPro.textInfo;
        TMP_CharacterInfo charInfo = textInfo.characterInfo[index];

        // Изменяем позицию буквы
        Vector3[] vertices = textMeshPro.mesh.vertices;
        int vertexIndex = charInfo.vertexIndex;

        if (vertices.Length <= 0)
            return;

        // Пример: сдвиг по X на 10 единиц
        vertices[vertexIndex] += (Vector3)offset;
        vertices[vertexIndex + 1] += (Vector3)offset;
        vertices[vertexIndex + 2] += (Vector3)offset;
        vertices[vertexIndex + 3] += (Vector3)offset;

        // Применяем изменения к вершинам текста
        textMeshPro.mesh.vertices = vertices;
        textMeshPro.canvasRenderer.SetMesh(textMeshPro.mesh);
    }

    private IEnumerator AnimationCoroutine()
    {
        float timeoffset = 0.25f;
        float currentOffset = 0;
        float time = 2f;
        float speed = 1f;
        List<bool> dir = new List<bool>();
        List<float> times = new List<float>();

        int size = 0;

        while (true)
        {
            yield return new WaitForFixedUpdate();

            for (int i = 0; i < size; i++)
            {
                TranslateCharacterPosition(i, new Vector2(0, speed * (dir[i] ? 1 : -1)));

                times[i] -= Time.fixedDeltaTime;

                if (times[i] < 0)
                {
                    times[i] = time;

                    dir[i] = !dir[i];
                }
            }

            if (size < textMeshPro.text.Length)
            {
                currentOffset += Time.fixedDeltaTime;

                if (currentOffset < timeoffset)
                    continue;

                currentOffset = 0;

                dir.Add(true);
                times.Add(time / 2);

                size++;
            }
            else
            {
                dir.Remove(dir.Last());
                times.Remove(times.Last());

                size--;
            }
            

        }
    }
}