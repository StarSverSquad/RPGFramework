using DG.Tweening;
using RPGF.GUI;
using RPGF.GUI.Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GlackSaga.GUI.TitleMenu.Main
{
    public class TittleMenuMainBlock : GUIChoiceBlock
    {
        [SerializeField]
        private RectTransform _panel;
        [SerializeField]
        private float _panelHideOffset = 110f;
        [SerializeField]
        private float _panelAnimationTime = 0.5f;
        [SerializeField]
        private float _wordsBgTransparency = 0.5f;
        [Space]
        [SerializeField]
        private MoneyGUI _money;
        [SerializeField]
        private Image _solidColorBg;
        [SerializeField]
        private Image _wordsBg;
        [SerializeField]
        private float _solidColorBgAlpha;
        [SerializeField]
        private float _bgAnimtionTime = 0.5f;

        [SerializeField]
        private CharacterInformationManager _characterInformation;

        private Coroutine openAnimation;
        private Coroutine closeAnimation;

        private Tween panelTween;
        private Tween solidClrTween;

        public override void Initialize(IGUIManager manager)
        {
            base.Initialize(manager);

            _panel.anchoredPosition = new Vector2(0, _panelHideOffset);

            _wordsBg.color = new Color(_wordsBg.color.r, _wordsBg.color.g, _wordsBg.color.b, 0);

            Color solidClr = _solidColorBg.color; solidClr.a = 0;
            _solidColorBg.color = solidClr;
        }

        protected override void OnFocus()
        {
            if (openAnimation != null)
                StopCoroutine(openAnimation);

            openAnimation = StartCoroutine(OpenAnimation());

            _characterInformation.Initialize();
            _characterInformation.Show();
        }

        protected override void OnLostFocus()
        {
            _characterInformation.Hide();
        }

        public override void OnCanceled()
        {
            if (closeAnimation != null)
                StopCoroutine(closeAnimation);

            closeAnimation = StartCoroutine(CloseAnimation());
        }

        private IEnumerator OpenAnimation()
        {
            DisposeTweens();

            _money.Initialize();

            yield return new WaitForFixedUpdate();

            panelTween = _panel.DOAnchorPosY(0, _panelAnimationTime)
                .SetEase(Ease.OutSine)
                .Play();

            Color solidClr = _solidColorBg.color; solidClr.a = _solidColorBgAlpha;
            solidClrTween = _solidColorBg.DOColor(solidClr, _bgAnimtionTime).Play();

            _wordsBg.DOColor(
                new Color(_wordsBg.color.r, _wordsBg.color.g, _wordsBg.color.b, _wordsBgTransparency), _bgAnimtionTime)
                .Play();

            yield return new WaitForSeconds(_bgAnimtionTime);

            DisposeTweens();

            StartChoice();

            openAnimation = null;
        }

        private IEnumerator CloseAnimation()
        {
            DisposeTweens();

            _money.Dispose();

            yield return new WaitForFixedUpdate();

            Color solidClr = _solidColorBg.color; solidClr.a = 0;
            solidClrTween = _solidColorBg.DOColor(solidClr, _bgAnimtionTime).Play();

            _wordsBg.DOColor(
                new Color(_wordsBg.color.r, _wordsBg.color.g, _wordsBg.color.b, 0), _bgAnimtionTime)
                .Play();

            panelTween = _panel
                .DOAnchorPosY(_panelHideOffset, _panelAnimationTime)
                .SetEase(Ease.InSine)
                .Play();

            _characterInformation.Hide();

            yield return new WaitForSeconds(_bgAnimtionTime);

            DisposeTweens();

            Preview();

            closeAnimation = null;
        }

        private void DisposeTweens()
        {
            panelTween?.Kill();
            solidClrTween?.Kill();

            panelTween = null;
            solidClrTween = null;
        }

        protected override void OnDisable()
        {
            DisposeTweens();
        }
    }

}