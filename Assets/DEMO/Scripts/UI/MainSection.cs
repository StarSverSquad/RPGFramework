using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainSection : UISectionBase
{
    [Space]
    public TextMeshProUGUI Dmg;
    public TextMeshProUGUI Def;
    public TextMeshProUGUI Agi;
    public TextMeshProUGUI Luck;

    public TextMeshProUGUI Weapon;
    public TextMeshProUGUI Head;
    public TextMeshProUGUI Body;
    public TextMeshProUGUI Shield;
    public TextMeshProUGUI Talisman;

    public TextMeshProUGUI CharacterName;
    public TextMeshProUGUI CharacterLevel;
    public TextMeshProUGUI CharacterClass;

    public NumeriticBar HpBar;
    public NumeriticBar MpBar;

    public Image CharacterIcon;

    public override void Initialize()
    {
        base.Initialize();
    }

    public void UpdateInfo()
    {
        if (GameManager.Instance.character.Characters.Length == 0)
            return;

        RPGCharacter character = GameManager.Instance.character.Characters[0];

        HpBar.SetValue(character.Heal, character.MaxHeal);
        MpBar.SetValue(character.Mana, character.MaxMana);

        CharacterIcon.sprite = character.Icon;
        CharacterClass.text = character.Class;
        CharacterName.text = character.Name;
        CharacterLevel.text = $"Статус {character.Level}";

        Dmg.text = $"Атака: {character.Damage}";
        Def.text = $"Защита: {character.Defence}";
        Agi.text = $"Ловкость: {character.Agility}";
        Luck.text = $"Удача: {character.Luck}";

        if (character.WeaponSlot != null)
            Weapon.text = $"Оружие: {character.WeaponSlot.Name}";
        else
            Weapon.text = $"Оружие: ПУСТО";

        if (character.HeadSlot != null)
            Head.text = $"Голова: {character.HeadSlot.Name}";
        else
            Head.text = $"Голова: ПУСТО";

        if (character.BodySlot != null)
            Body.text = $"Тело: {character.BodySlot.Name}";
        else
            Body.text = $"Тело: ПУСТО";

        if (character.ShieldSlot != null)
            Shield.text = $"Щит: {character.ShieldSlot.Name}";
        else
            Shield.text = $"Щит: ПУСТО";

        if (character.TalismanSlot != null)
            Talisman.text = $"Талисман: {character.TalismanSlot.Name}";
        else
            Talisman.text = $"Талисман: ПУСТО";
    }
}
