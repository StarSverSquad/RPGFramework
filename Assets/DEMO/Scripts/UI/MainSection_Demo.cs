using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainSection_Demo : UISectionBase
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

    public void ExitToMainMenu()
    {
        GameManager.Instance.sceneLoader.LoadScene("DemoMenu");
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
        CharacterName.text = character.Tag;
        CharacterLevel.text = $"������ {character.Level}";

        Dmg.text = $"�����: {character.Damage}";
        Def.text = $"������: {character.Defence}";
        Agi.text = $"��������: {character.Agility}";
        Luck.text = $"�����: {character.Luck}";

        if (character.WeaponSlot != null)
            Weapon.text = $"������: {character.WeaponSlot.Tag}";
        else
            Weapon.text = $"������: �����";

        if (character.HeadSlot != null)
            Head.text = $"������: {character.HeadSlot.Tag}";
        else
            Head.text = $"������: �����";

        if (character.BodySlot != null)
            Body.text = $"����: {character.BodySlot.Tag}";
        else
            Body.text = $"����: �����";

        if (character.ShieldSlot != null)
            Shield.text = $"���: {character.ShieldSlot.Tag}";
        else
            Shield.text = $"���: �����";

        if (character.TalismanSlot != null)
            Talisman.text = $"��������: {character.TalismanSlot.Tag}";
        else
            Talisman.text = $"��������: �����";
    }
}
