using DG.Tweening;
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
    private TextMeshProUGUI healCounter;

    [SerializeField]
    private TextMeshProUGUI manaCounter;

    [SerializeField]
    private LineBar healBar;

    [SerializeField]
    private LineBar manaBar;

    // <-- -->
    private Sequence animation0;

    public void Initialize(BattleCharacterInfo character)
    {
        gradient.color = character.Character.Color;
        icon.sprite = character.Character.Icon;

        characterNameTxt.text = character.Character.Name;

        healCounter.text = $"{character.Heal} / {character.Character.MaxHeal}";
        manaCounter.text = $"{character.Mana} / {character.Character.MaxMana}";

        healBar.SetValue((float)character.Heal / (float)character.Character.MaxHeal);
        manaBar.SetValue((float)character.Mana / (float)character.Character.MaxMana);
    }

    public void StartAnimation()
    {
        animation0?.Kill(true);

        animation0 = DOTween.Sequence();

        animation0.Append(content.DOAnchorPosX(25, 0.75f).SetEase(Ease.OutSine));

        animation0.Append(content.DOAnchorPosX(0, 0.75f).SetEase(Ease.OutSine));

        animation0.SetLoops(-1).SetDelay(0.5f).Play();
    }

    public void StopAnimation()
    {
        animation0.Kill(true);
    }
}
