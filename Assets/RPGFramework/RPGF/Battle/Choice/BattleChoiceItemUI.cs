using DG.Tweening;
using RPGF.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPGF.Battle.Choice
{
    public class BattleChoiceItemUI : RPGFrameworkBehaviour
    {
        public TextMeshProUGUI MainText;
        public TextMeshProUGUI CounterText;
        public Image Icon;

        [SerializeField]
        private RectTransform[] FocusEffectElements = new RectTransform[2];

        [SerializeField]
        private Color commonColor;
        [SerializeField]
        private Color lockedColor;

        [SerializeField]
        private Color textCommonColor;
        [SerializeField]
        private Color textFocusColor;

        private Color actualColor;

        private Tween[] aS = new Tween[2];
        private Tween[] bS = new Tween[2];

        private Vector2[] startSizesOfFocusEffect = new Vector2[2];

        public override void Initialize()
        {
            actualColor = commonColor;

            startSizesOfFocusEffect[0] = FocusEffectElements[0].sizeDelta;
            startSizesOfFocusEffect[1] = FocusEffectElements[1].sizeDelta;
        }

        public void SetIcon(Sprite sprite)
        {
            var current = MainText.margin;

            if (sprite is not null)
            {
                Icon.enabled = true;
                Icon.sprite = sprite;

                current.x = 65;
                MainText.margin = current;
            }
            else
            {
                Icon.enabled = false;
                current.x = 10;
                MainText.margin = current;
            }
        }

        public void UnLock()
        {
            actualColor = commonColor;
        }

        public void Lock()
        {
            actualColor = lockedColor;
        }

        public void UnFocus()
        {
            MainText.color = textCommonColor;

            aS[0].Kill(true);
            aS[1].Kill(true);

            bS[0].Kill(true);
            bS[1].Kill(true);

            FocusEffectElements[0].sizeDelta = startSizesOfFocusEffect[0];
            FocusEffectElements[1].sizeDelta = startSizesOfFocusEffect[1];

            FocusEffectElements[0].GetComponent<Image>().enabled = false;
            FocusEffectElements[1].GetComponent<Image>().enabled = false;
        }

        public void Focus()
        {
            FocusEffectElements[0].GetComponent<Image>().enabled = true;
            FocusEffectElements[1].GetComponent<Image>().enabled = true;

            MainText.color = textFocusColor;

            Color outColor = actualColor;
            outColor.a = 0;

            aS[0] = FocusEffectElements[0]
                .DOSizeDelta(new Vector2(0, 60), 1f)
                .From(new Vector2(0, 0))
                .SetLoops(-1)
                .Play();

            aS[1] = FocusEffectElements[0].GetComponent<Image>()
                .DOColor(outColor, 1f)
                .From(actualColor)
                .SetLoops(-1)
                .Play();

            bS[0] = FocusEffectElements[1]
                .DOSizeDelta(new Vector2(0, 60), 1f)
                .From(new Vector2(0, 0))
                .SetDelay(0.5f)
                .SetLoops(-1)
                .Play();

            bS[1] = FocusEffectElements[1].GetComponent<Image>()
                .DOColor(outColor, 1f)
                .SetDelay(0.5f)
                .From(actualColor)
                .SetLoops(-1)
                .Play();
        }

        private void OnDestroy()
        {
            aS[0].Kill(true);
            aS[1].Kill(true);

            bS[0].Kill(true);
            bS[1].Kill(true);
        }
    }
}