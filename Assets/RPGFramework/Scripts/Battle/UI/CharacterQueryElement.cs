using DG.Tweening;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterQueryElement : MonoBehaviour
{
    [SerializeField]
    private RectTransform content;

    [SerializeField]
    private Image gradient;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private TextMeshProUGUI characterNameTxt;

    [SerializeField]
    private IconList iconList;
    [SerializeField]
    private LineBar healBar;
    [SerializeField]
    private LineBar manaBar;

    [SerializeField]
    private float moveDuration = 0.2f;

    private RectTransform rectTransform;

    // <-- -->
    private Sequence animation0;
    private Tween moveAnimation;

    public void Initialize(RPGCharacter character)
    {
        gradient.color = character.Color;
        icon.sprite = character.BattleSmallImage;

        characterNameTxt.text = character.Name;

        iconList.UpdateIcons(character.States.Select(i => i.Icon).ToArray());

        healBar.SetValue((float)character.Heal / (float)character.MaxHeal);
        manaBar.SetValue((float)character.Mana / (float)character.MaxMana);

        rectTransform = GetComponent<RectTransform>();
    }

    public void StartAnimation()
    {
        animation0?.Kill(true);

        animation0 = DOTween.Sequence();

        animation0.Append(content.DOAnchorPosX(25, 0.75f).SetEase(Ease.OutSine));

        animation0.Append(content.DOAnchorPosX(0, 0.75f).SetEase(Ease.OutSine));

        animation0.SetLoops(-1).SetDelay(0.5f).Play();
    }

    public void MoveToPoint(Vector2 rectPoint, Vector2 fromPoint)
    {
        if (moveAnimation != null)
            moveAnimation.Kill();

        rectTransform.anchoredPosition = fromPoint;

        moveAnimation = rectTransform.DOAnchorPos(rectPoint, moveDuration).Play();
    }

    public void StopAnimation()
    {
        animation0.Kill(true);
    }

    private void OnDestroy()
    {
        animation0.Kill();
        moveAnimation.Kill();
    }
}
