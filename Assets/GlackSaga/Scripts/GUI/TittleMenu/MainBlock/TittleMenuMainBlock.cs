using DG.Tweening;
using RPGF.GUI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TittleMenuMainBlock : GUIChoicableBlock
{
    [SerializeField]
    private RectTransform _panel;
    [SerializeField]
    private float _panelHideOffset = 110f;
    [SerializeField]
    private float _panelAnimationTime = 0.5f;

    [Space]
    [SerializeField]
    private MoneyGUI _money;
    [SerializeField]
    private Image _solidColorBg;
    [SerializeField]
    private RectTransform _wordsBg;
    [SerializeField]
    private float _solidColorBgAlpha;
    [SerializeField]
    private Vector2 _wordsBgOutPosition;
    [SerializeField]
    private float _bgAnimtionTime = 0.5f;

    [SerializeField]
    private CharacterInformationManager _characterInformation;

    private Coroutine openAnimation;
    private Coroutine closeAnimation;

    private Tween bgTween;
    private Tween panelTween;
    private Tween solidClrTween;

    public override void Initialize(GUIManagerBase manager)
    {
        base.Initialize(manager);

        _panel.anchoredPosition = new Vector2(0, _panelHideOffset);

        _wordsBg.anchoredPosition = _wordsBgOutPosition;

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

        bgTween = _wordsBg.DOAnchorPos(Vector2.zero, _bgAnimtionTime).Play();

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

        bgTween = _wordsBg.DOAnchorPos(_wordsBgOutPosition, _bgAnimtionTime).Play();

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
        bgTween?.Kill();
        panelTween?.Kill();
        solidClrTween?.Kill();

        bgTween = null;
        panelTween = null;
        solidClrTween = null;
    }

    protected override void OnDisable()
    {
        DisposeTweens();
    }
}
