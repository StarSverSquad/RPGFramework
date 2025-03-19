using DG.Tweening;
using RPGF.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TittleMenuMainButton : GUIElementBase
{
    [Space]
    [SerializeField]
    private TextMeshProUGUI _textMesh;
    [SerializeField]
    private Color _unfocusedColor;
    [SerializeField]
    private Color _focusedColor;
    [SerializeField]
    private Color _selectedColor;
    [Space]
    [SerializeField]
    private RectTransform _underline;
    [SerializeField]
    private float _underlineAnimationTime;

    private Image underlineImage;

    private void OnEnable()
    {
        _underline.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);

        underlineImage = _underline.GetComponent<Image>();

        underlineImage.color = _focusedColor;
    }

    public override void Select()
    {
        base.Select();

        _textMesh.color = _selectedColor;
        underlineImage.color = _selectedColor;
    }

    public override void SetFocus(bool focus)
    {
        base.SetFocus(focus);

        underlineImage.color = _focusedColor;

        if (focus)
        {
            _textMesh.color = _focusedColor;
            _underline.DOKill();
            _underline
                .DOSizeDelta(new Vector2(_textMesh.GetPreferredValues().x, _underline.sizeDelta.y), _underlineAnimationTime)
                .Play();
        }
        else
        {
            _textMesh.color = _unfocusedColor;
            _underline.DOKill();
            _underline
                .DOSizeDelta(new Vector2(0, _underline.sizeDelta.y), _underlineAnimationTime)
                .Play();
        }
    }

    private void OnDisable()
    {
        _underline.DOKill();
    }
}
