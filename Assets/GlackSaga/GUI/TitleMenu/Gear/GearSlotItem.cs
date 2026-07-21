using DG.Tweening;
using RPGF.Domain.Interfaces;
using RPGF.GUI;
using RPGF.RPG;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GlackSaga.GUI.TitleMenu.Gear
{
    public class GearSlotItem : GUIInteractable, ISetData<RPGWearable>
    {
        [Header("LINKS:")]
        [SerializeField]
        private TextMeshProUGUI nameText;
        [SerializeField]
        private Image background;

        [Header("OPTIONS:")]
        [SerializeField]
        private RPGWearable.UsedType usedType;
        [SerializeField]
        private Color focusedColor = Color.orange;
        [SerializeField]
        private Color unfocusedColor = Color.white;

        private Sequence selectionTween;

        public RPGWearable.UsedType UsedType => usedType;

        public void SetData(RPGWearable value)
        {
            if (value == null)
            {
                nameText.text = GetLocale("SYS_EMPTY");
                nameText.color = Color.gray;
                return;
            }

            nameText.color = unfocusedColor;
            nameText.text = GetLocale(value.GetLocaleNameTag(), value.Name);
        }

        public override void OnFocused()
        {
            base.OnFocused();

            selectionTween = DOTween.Sequence();

            selectionTween
                .Append(background.DOColor(focusedColor, 0.35f).From(unfocusedColor))
                .Append(background.DOColor(unfocusedColor, 0.35f))
                .SetEase(Ease.Linear)
                .SetLoops(-1);

            selectionTween.Play();
        }

        public override void OnSelected()
        {
            base.OnSelected();

            selectionTween.Kill();

            background.color = focusedColor;
        }

        public override void OnUnfocused()
        {
            base.OnUnfocused();

            selectionTween.Kill();

            background.color = unfocusedColor;
        }
    }
}
