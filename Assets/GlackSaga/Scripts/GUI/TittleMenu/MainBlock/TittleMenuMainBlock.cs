using DG.Tweening;
using RPGF.GUI;
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
    private Image _solidColorBg;
    [SerializeField]
    private RectTransform _wordsBg;
    [SerializeField]
    private float _solidColorBgAlpha;
    [SerializeField]
    private Vector2 _wordsBgOutPosition;
    [SerializeField]
    private float _bgAnimtionTime = 0.5f;

    public override void Initialize(GUIManagerBase manager)
    {
        base.Initialize(manager);

        _panel.anchoredPosition = new Vector2(0, _panelHideOffset);

        _wordsBg.anchoredPosition = _wordsBgOutPosition;

        Color solidClr = _solidColorBg.color; solidClr.a = 0;
        _solidColorBg.color = solidClr;
    }

    protected override void OnActivate()
    {
        _panel.DOKill();
        _panel.DOAnchorPosY(0, _panelAnimationTime)
            .SetEase(Ease.OutSine)
            .Play();

        _solidColorBg.DOKill();
        Color solidClr = _solidColorBg.color; solidClr.a = _solidColorBgAlpha;
        _solidColorBg.DOColor(solidClr, _bgAnimtionTime).Play();

        _wordsBg.DOKill();
        _wordsBg.DOAnchorPos(Vector2.zero, _bgAnimtionTime).Play();

        base.OnActivate();
    }

    public override void OnCanceled()
    {
        _solidColorBg.DOKill();
        Color solidClr = _solidColorBg.color; solidClr.a = 0;
        _solidColorBg.DOColor(solidClr, _bgAnimtionTime).Play();

        _wordsBg.DOKill();
        _wordsBg.DOAnchorPos(_wordsBgOutPosition, _bgAnimtionTime).Play();

        _panel.DOKill();
        var panelAnim = _panel
            .DOAnchorPosY(_panelHideOffset, _panelAnimationTime)
            .SetEase(Ease.InSine)
            .Play();

        panelAnim.onComplete += () =>
        {
            Preview();
        };
    }
}
