using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleCharacterPreview : MonoBehaviour, IActive
{
    [SerializeField]
    private GameObject container;

    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private NumeriticBar healBar;
    [SerializeField]
    private NumeriticBar manaBar;

    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private TextMeshProUGUI stastText;

    private void Start()
    {
        SetActive(false);
    }

    public void SetActive(bool active) => container.SetActive(active);

    public void SetData(RPGCharacter character)
    {
        icon.sprite = character.Icon;

        nameText.text = character.Name;

        healBar.SetValue(character.Heal, character.MaxHeal);
        manaBar.SetValue(character.Mana, character.MaxMana);

        levelText.text = $"Уровень {character.Level}";

        stastText.text = $"Урон: {character.Damage}\n" +
                         $"Защита: {character.Defence}\n" +
                         $"Ловкость: {character.Agility}\n" +
                         $"Удача: {character.Luck}";
    }
}