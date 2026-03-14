using RPGF.Domain.Interfaces;
using System.Linq;
using TMPro;
using UnityEngine;

namespace RPGF.Core.Services
{
    public class TransformTextMeshService : IService
    {
        private readonly TextMeshProUGUI textMesh;

        public TMP_CharacterInfo[] Visibles { get; private set; }
        public TMP_CharacterInfo[] AllCharacters { get; private set; }

        public int CharactersCount => Visibles.Length;
        public int VerticesCount => Vertices.Length;
        public int ColorsCount => Colors.Length;

        public Color[] Colors { get; private set; }
        public Vector3[] Vertices { get; private set; }

        private Mesh mesh;

        // Кэш оригинальных вершин и цветов
        private Vector3[] originalVertices;
        private Color[] originalColors;

        public TransformTextMeshService(TextMeshProUGUI textMesh)
        {
            this.textMesh = textMesh;
            InitializeMesh();
        }

        // Инициализация меша (вызывается один раз)
        private void InitializeMesh()
        {
            textMesh.ForceMeshUpdate();

            AllCharacters = textMesh.textInfo.characterInfo;
            Visibles = AllCharacters.Where(c => c.isVisible).ToArray();
            mesh = textMesh.mesh;

            if (mesh == null) return;

            Vertices = mesh.vertices.Clone() as Vector3[];
            Colors = mesh.colors.Clone() as Color[];

            // Сохраняем оригинальные данные для быстрого восстановления
            originalVertices = mesh.vertices.Clone() as Vector3[];
            originalColors = mesh.colors.Clone() as Color[];
        }

        // Полное обновление всего меша
        public void ResetMesh()
        {
            textMesh.ForceMeshUpdate();

            AllCharacters = textMesh.textInfo.characterInfo;
            Visibles = AllCharacters.Where(c => c.isVisible).ToArray();
            mesh = textMesh.mesh;

            if (mesh == null) return;

            Vertices = mesh.vertices.Clone() as Vector3[];
            Colors = mesh.colors.Clone() as Color[];

            originalVertices = mesh.vertices.Clone() as Vector3[];
            originalColors = mesh.colors.Clone() as Color[];
        }

        public void ResetMeshOnlyChanges(int from, int to)
        {
            int oldVisiblesLength = Visibles.Length;

            AllCharacters = textMesh.textInfo.characterInfo;
            Visibles = AllCharacters.Where(c => c.isVisible).ToArray();
            mesh = textMesh.mesh;

            if (mesh == null) return;

            originalVertices = mesh.vertices.Clone() as Vector3[];
            originalColors = mesh.colors.Clone() as Color[];

            UpdateMeshForCharacters(from, to);
        }

        // Частичное обновление только указанных символов (по исходным индексам)
        public void PartialResetMesh(int startIndex, int endIndex)
        {
            textMesh.ForceMeshUpdate();
            AllCharacters = textMesh.textInfo.characterInfo;
            Visibles = AllCharacters.Where(c => c.isVisible).ToArray();

            if (mesh == null) return;

            // Обновляем только символы в указанном диапазоне
            for (int charIndex = startIndex; charIndex <= endIndex && charIndex < AllCharacters.Length; charIndex++)
            {
                UpdateSingleCharacter(charIndex);
            }
        }

        // Обновление только видимых символов в диапазоне
        public void PartialResetVisibleMesh(int startVisibleIndex, int endVisibleIndex)
        {
            textMesh.ForceMeshUpdate();
            AllCharacters = textMesh.textInfo.characterInfo;
            Visibles = AllCharacters.Where(c => c.isVisible).ToArray();

            if (mesh == null) return;

            // Обновляем только видимые символы в диапазоне
            for (int visibleIndex = startVisibleIndex;
                 visibleIndex <= endVisibleIndex && visibleIndex < Visibles.Length;
                 visibleIndex++)
            {
                UpdateSingleCharacter(visibleIndex);
            }
        }

        // Обновить один символ по исходному индексу
        public void UpdateSingleCharacter(int originalIndex)
        {
            if (originalIndex < 0 || originalIndex >= AllCharacters.Length)
                return;

            var charInfo = AllCharacters[originalIndex];
            if (!charInfo.isVisible || charInfo.vertexIndex < 0)
                return;

            // Получаем обновленные данные из меша
            var meshInfo = textMesh.textInfo.meshInfo[charInfo.materialReferenceIndex];

            // Обновляем 4 вершины этого символа
            for (int i = 0; i < 4; i++)
            {
                int vertexIndex = charInfo.vertexIndex + i;
                if (vertexIndex < Vertices.Length && vertexIndex < meshInfo.vertices.Length)
                {
                    Vertices[vertexIndex] = meshInfo.vertices[vertexIndex];
                }
            }

            // Обновляем 4 цвета этого символа
            for (int i = 0; i < 4; i++)
            {
                int vertexIndex = charInfo.vertexIndex + i;
                if (vertexIndex < Colors.Length && vertexIndex < meshInfo.colors32.Length)
                {
                    Colors[vertexIndex] = meshInfo.colors32[vertexIndex];
                }
            }
        }

        // Методы трансформации (работают с текущими вершинами, не обновляют их из TMP)
        public void SetCharacterPosition(int visibleIndex, Vector3 position)
        {
            if (!IsVisibleIndexValid(visibleIndex)) return;

            TMP_CharacterInfo characterInfo = Visibles[visibleIndex];

            // Сдвигаем все 4 вершины символа
            for (int i = 0; i < 4; i++)
            {
                int vertexIndex = characterInfo.vertexIndex + i;
                if (vertexIndex < Vertices.Length)
                {
                    Vertices[vertexIndex].x += position.x;
                    Vertices[vertexIndex].y += position.y;
                    Vertices[vertexIndex].z += position.z;
                }
            }
        }

        public void SetCharacterPositionByOriginalIndex(int originalIndex, Vector3 position)
        {
            // Находим видимый индекс
            for (int i = 0; i < Visibles.Length; i++)
            {
                if (Visibles[i].vertexIndex == AllCharacters[originalIndex].vertexIndex)
                {
                    SetCharacterPosition(i, position);
                    return;
                }
            }
        }

        public void SetCharacterColor(int visibleIndex, Color32 color)
        {
            if (!IsVisibleIndexValid(visibleIndex)) return;

            TMP_CharacterInfo characterInfo = Visibles[visibleIndex];

            for (int i = 0; i < 4; i++)
            {
                int vertexIndex = characterInfo.vertexIndex + i;
                if (vertexIndex < Colors.Length)
                {
                    Colors[vertexIndex] = color;
                }
            }
        }

        public void SetCharacterColorByOriginalIndex(int originalIndex, Color32 color)
        {
            for (int i = 0; i < Visibles.Length; i++)
            {
                if (Visibles[i].vertexIndex == AllCharacters[originalIndex].vertexIndex)
                {
                    SetCharacterColor(i, color);
                    return;
                }
            }
        }

        // Сбросить один символ к оригинальному состоянию
        public void ResetCharacterToOriginal(int originalIndex)
        {
            if (originalIndex < 0 || originalIndex >= AllCharacters.Length)
                return;

            var charInfo = AllCharacters[originalIndex];
            if (!charInfo.isVisible || charInfo.vertexIndex < 0)
                return;

            // Восстанавливаем оригинальные вершины и цвета
            for (int i = 0; i < 4; i++)
            {
                int vertexIndex = charInfo.vertexIndex + i;
                if (vertexIndex < Vertices.Length && vertexIndex < originalVertices.Length)
                {
                    Vertices[vertexIndex] = originalVertices[vertexIndex];
                }

                if (vertexIndex < Colors.Length && vertexIndex < originalColors.Length)
                {
                    Colors[vertexIndex] = originalColors[vertexIndex];
                }
            }
        }

        // Сбросить диапазон символов к оригинальному состоянию
        public void ResetCharactersToOriginal(int startIndex, int endIndex)
        {
            for (int i = startIndex; i <= endIndex && i < AllCharacters.Length; i++)
            {
                ResetCharacterToOriginal(i);
            }
        }

        // Применить изменения к мешу
        public void UpdateMesh()
        {
            if (mesh != null)
            {
                mesh.vertices = Vertices;
                mesh.colors = Colors;
                textMesh.canvasRenderer.SetMesh(mesh);
            }
        }

        // Применить изменения только для диапазона вершин
        public void UpdatePartialMesh(int startVertexIndex, int endVertexIndex)
        {
            if (mesh == null) return;

            var tempVertices = mesh.vertices.Clone() as Vector3[];
            var tempColors = mesh.colors.Clone() as Color[];

            // Копируем только измененный диапазон вершин
            for (int i = startVertexIndex; i <= endVertexIndex && i < mesh.vertices.Length; i++)
            {
                tempVertices[i].x = Vertices[i].x;
                tempVertices[i].y = Vertices[i].y;
                tempVertices[i].z = Vertices[i].z;
            }
            mesh.vertices = tempVertices;

            // Копируем только измененный диапазон цветов
            for (int i = startVertexIndex; i <= endVertexIndex && i < mesh.colors.Length; i++)
            {
                tempColors[i].a = Colors[i].a;
                tempColors[i].r = Colors[i].r;
                tempColors[i].g = Colors[i].g;
                tempColors[i].b = Colors[i].b;
            }
            mesh.colors = tempColors;

            textMesh.canvasRenderer.SetMesh(mesh);
        }

        // Быстрое обновление меша для определенных символов
        public void UpdateMeshForCharacters(int startCharIndex, int endCharIndex)
        {
            if (mesh == null) return;

            // Находим диапазон вершин для этих символов
            int startVertexIndex = int.MaxValue;
            int endVertexIndex = int.MinValue;

            for (int i = startCharIndex; i <= endCharIndex && i < AllCharacters.Length; i++)
            {
                var charInfo = AllCharacters[i];
                if (charInfo.isVisible && charInfo.vertexIndex >= 0)
                {
                    startVertexIndex = Mathf.Min(startVertexIndex, charInfo.vertexIndex);
                    endVertexIndex = Mathf.Max(endVertexIndex, charInfo.vertexIndex + 3);
                }
            }

            if (startVertexIndex <= endVertexIndex)
            {
                UpdatePartialMesh(startVertexIndex, endVertexIndex);
            }
        }

        private bool IsVisibleIndexValid(int index)
        {
            return textMesh.textInfo != null &&
                   Visibles != null &&
                   Visibles.Length > 0 &&
                   index >= 0 &&
                   index < Visibles.Length
                   && originalVertices[Visibles[index].vertexIndex] != Vector3.zero;
        }

        // Утилиты для отладки
        public void LogCharacterRangeInfo(int startIndex, int endIndex)
        {
            Debug.Log($"Updating characters {startIndex}-{endIndex}");

            for (int i = startIndex; i <= endIndex && i < AllCharacters.Length; i++)
            {
                var charInfo = AllCharacters[i];
                Debug.Log($"Character [{i}]: '{charInfo.character}' " +
                         $"Visible: {charInfo.isVisible} " +
                         $"VertexIndex: {charInfo.vertexIndex}");
            }
        }
    }
}