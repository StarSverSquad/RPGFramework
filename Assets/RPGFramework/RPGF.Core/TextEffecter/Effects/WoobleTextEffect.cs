using RPGF.Core.TextEffecter.Abstractions;
using System.Collections;
using TMPro;
using UnityEngine;

namespace RPGF.Core.TextEffecter.Effects
{
    [UseTextEffect(Title = "Вубл", CodeName = "wooble")]
    public class WoobleTextEffect : TextEffectBase
    {
        public WoobleTextEffect(TextMeshProUGUI textMesh) : base(textMesh)
        {
        }

        protected override IEnumerator Pipeline()
        {
            while (true)
            {
                TextTransformer.ResetCharactersToOriginal(StartLetter, EndLetter);
                for (int i = StartLetter; i <= EndLetter; i++)
                {
                    Vector3 offset = Wobble(Time.time + i);

                    TextTransformer.SetCharacterPositionByOriginalIndex(i, offset);
                }
                TextTransformer.UpdateMeshForCharacters(StartLetter, EndLetter);

                yield return new WaitForFixedUpdate();
            }
        }

        private Vector2 Wobble(float time)
        {
            return new Vector2(Mathf.Sin(time * 3.3f), Mathf.Cos(time * 2.5f));
        }
    }
}