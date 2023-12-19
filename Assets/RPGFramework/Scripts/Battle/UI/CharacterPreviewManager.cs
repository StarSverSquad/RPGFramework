using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPreviewManager : MonoBehaviour, IActive
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
    }
}