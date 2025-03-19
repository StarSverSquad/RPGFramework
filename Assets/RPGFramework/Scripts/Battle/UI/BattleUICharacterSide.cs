using DG.Tweening;
using RPGF.RPG;
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

    public float TraslationTime => 0.7f;

    public void Setup(RPGCharacter battleCharacterInfo)
    {
        charName.text = battleCharacterInfo.Name;

        hpBar.SetValue((float)battleCharacterInfo.Heal / (float)battleCharacterInfo.MaxHeal);
        mnBar.SetValue((float)battleCharacterInfo.Mana / (float)battleCharacterInfo.MaxMana);

        hpCounter.text = $"{battleCharacterInfo.Heal} / {battleCharacterInfo.MaxHeal}";
        mnCounter.text = $"{battleCharacterInfo.Mana} / {battleCharacterInfo.MaxMana}";

        icon.sprite = battleCharacterInfo.BattleImage;

        effectList.UpdateIcons(battleCharacterInfo.States.Select(i => i.Icon).ToArray());
    }

    public void Show()
    {
        rect.DOAnchorPosX(210, TraslationTime).SetEase(Ease.OutExpo).Play();
    }

    public void Hide()
    {
        rect.DOAnchorPosX(-350, TraslationTime).SetEase(Ease.OutExpo).Play();             
    }
}