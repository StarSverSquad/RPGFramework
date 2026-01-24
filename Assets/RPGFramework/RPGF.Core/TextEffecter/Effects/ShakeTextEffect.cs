using RPGF.Core.TextEffecter.Abstractions;
using System.Collections;
using TMPro;
using UnityEngine;

namespace RPGF.Core.TextEffecter.Effects
{
    [UseTextEffect(Title = "Дрожание", CodeName = "shake")]
    public class ShakeTextEffect : TextEffectBase
    {
        public ShakeTextEffect(TextMeshProUGUI textMesh) : base(textMesh)
        {
        }

        protected override IEnumerator Pipeline()
        {
            while (true)
            {
                TextTransformer.ResetCharactersToOriginal(StartLetter, EndLetter);
                for (int i = StartLetter; i <= EndLetter; i++)
                {
                    TextTransformer.SetCharacterPositionByOriginalIndex(i, new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f)));
                }
                TextTransformer.UpdateMeshForCharacters(StartLetter, EndLetter);

                yield return new WaitForSeconds(.025f);
            }
        }
    }
}