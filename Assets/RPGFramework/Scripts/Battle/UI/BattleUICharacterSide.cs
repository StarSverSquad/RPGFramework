using DG.Tweening;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUICharacterSide : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI charName;
    [SerializeField]
    public TextMeshProUGUI hpCounter;
    [SerializeField]
    public TextMeshProUGUI mnCounter;

    [SerializeField]
    public LineBar hpBar;
    [SerializeField]
    public LineBar mnBar;

    [SerializeField]
    public Image icon;

    [SerializeField]
    public IconList effectList;

    [SerializeField]
    private RectTransform rect;

    public float TraslationTime => 1f;

    public void Setup(BattleCharacterInfo battleCharacterInfo)
    {
        charName.text = battleCharacterInfo.Character.Name;

        hpBar.SetValue(battleCharacterInfo.Character.Heal / battleCharacterInfo.Character.MaxHeal);
        mnBar.SetValue(battleCharacterInfo.Character.Mana / battleCharacterInfo.Character.MaxMana);

        hpCounter.text = $"{battleCharacterInfo.Character.Heal} / {battleCharacterInfo.Character.MaxHeal}";
        hpCounter.text = $"{battleCharacterInfo.Character.Mana} / {battleCharacterInfo.Character.MaxMana}";

        icon.sprite = battleCharacterInfo.Character.Icon;

        effectList.UpdateIcons(battleCharacterInfo.States.Select(i => i.Icon).ToArray());
    }

    public void Show()
    {
        rect.DOAnchorPosX(210, TraslationTime).SetLoops(0).SetEase(Ease.OutSine).Play();
    }

    public void Hide()
    {
        rect.DOAnchorPosX(-200, TraslationTime).SetLoops(0).SetEase(Ease.InSine).Play();             
    }
}