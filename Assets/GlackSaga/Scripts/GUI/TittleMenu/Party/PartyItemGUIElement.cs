using DG.Tweening;
using RPGF.GUI;
using RPGF.RPG;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GlackSaga.GUI.TitleMenu.Party
{
    public class PartyItemGUIElement : GUIElementBase, IDisposable
    {
        [Header("LINKS:")]
        [SerializeField]
        private Image _background;
        [SerializeField]
        private Image _image;
        [SerializeField]
        private TextMeshProUGUI _name;
        [SerializeField]
        private Image _gradient;

        [Header("OPTIONS:")]
        [SerializeField]
        private Color _focusBgColor = Color.white;
        [SerializeField]
        private Color _defaultBgColor = Color.black;
        [SerializeField]
        private float _bgAnimTime = 0.2f;

        private Sequence bgTween;

        public void SetData(RPGCharacter character)
        {
            _image.sprite = character.TitleImage;
            _gradient.color = character.Color;
            _name.text = GetLocale(character.Name);
        }

        public override void OnFocused()
        {
            Dispose();

            bgTween = DOTween.Sequence();

            bgTween
                .SetEase(Ease.Linear)
                .SetLoops(-1);

            bgTween
                .Append(_background
                    .DOColor(_focusBgColor, _bgAnimTime)
                    .From(_defaultBgColor));

            bgTween
                .Append(_background
                    .DOColor(_defaultBgColor, _bgAnimTime)
                    .From(_focusBgColor));

            bgTween.Play();
        }

        public override void OnUnfocused()
        {
            Dispose();
        }

        public override void OnCanceled()
        {
            Dispose();
        }

        private void DisposeTweens()
        {
            if (bgTween != null)
            {
                bgTween.Kill();
                bgTween = null;
            }
        }

        public void Dispose()
        {
            DisposeTweens();

            _background.color = _defaultBgColor;
        }
    }
}
