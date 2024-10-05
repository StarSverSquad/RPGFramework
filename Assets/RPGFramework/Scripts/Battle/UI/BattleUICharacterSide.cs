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

    public float TraslationTime => 0.7f;

    public void Setup(BattleCharacterInfo battleCharacterInfo)
    {
        charName.text = battleCharacterInfo.Character.Name;

        hpBar.SetValue((float)battleCharacterInfo.Heal / (float)battleCharacterInfo.Character.MaxHeal);
        mnBar.SetValue((float)battleCharacterInfo.Mana / (float)battleCharacterInfo.Character.MaxMana);

        hpCounter.text = $"{battleCharacterInfo.Heal} / {battleCharacterInfo.Character.MaxHeal}";
        mnCounter.text = $"{battleCharacterInfo.Mana} / {battleCharacterInfo.Character.MaxMana}";

        icon.sprite = battleCharacterInfo.Character.Icon;

        effectList.UpdateIcons(battleCharacterInfo.States.Select(i => i.Icon).ToArray());
    }

    public void Show()
    {
        rect.DOAnchorPosX(210, TraslationTime).SetEase(Ease.OutExpo).Play();
    }

    public void Hide()
    {
        rect.DOAnchorPosX(-200, TraslationTime).SetEase(Ease.OutExpo).Play();             
    }
}