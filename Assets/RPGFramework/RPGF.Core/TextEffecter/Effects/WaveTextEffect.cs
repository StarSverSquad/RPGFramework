using RPGF.Core.TextEffecter.Abstractions;
using System.Collections;
using TMPro;
using UnityEngine;

namespace RPGF.Core.TextEffecter.Effects
{
    [UseTextEffect(Title = "Волна", CodeName = "wave")]
    public class WaveTextEffect : TextEffectBase
    {
        public WaveTextEffect(TextMeshProUGUI textMesh) : base(textMesh)
        {
        }

        protected override IEnumerator Pipeline()
        {
            while (true)
            {
                TextTransformer.ResetCharactersToOriginal(StartLetter, EndLetter);
                for (int i = StartLetter; i <= EndLetter; i++)
                {
                    Vector3 offset = Wave(Time.time + (i + 1) / 5f);

                    TextTransformer.SetCharacterPositionByOriginalIndex(i, offset);
                }
                TextTransformer.UpdateMeshForCharacters(StartLetter, EndLetter);

                yield return new WaitForFixedUpdate();
            }
        }

        private Vector2 Wave(float time)
        {
            return new Vector2(Mathf.Cos(time) * 1.5f, Mathf.Sin(time * 7) * 5);
        }
    }
}