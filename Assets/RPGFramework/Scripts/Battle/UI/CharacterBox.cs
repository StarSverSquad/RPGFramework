using RPGF.RPG;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBox : RPGFrameworkBehaviour, IDisposable
{
    [SerializeField]
    private IconList iconList;

    [SerializeField]
    private Image characterImage;
    [SerializeField]
    private Image actImage;

    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private GameObject istargetobj;
    [SerializeField]
    private GameObject isdead;

    [SerializeField]
    private LineBar healBar;
    [SerializeField]
    private LineBar manaBar;

    public RPGCharacter Character { get; private set; }

    [SerializeField]
    private bool isDead = false;
    public bool IsDead => isDead;

    private bool initialized = false;

    public void Initialize(RPGCharacter character)
    {
        Character = character;

        character.OnManaChanged += UpdateMana;
        character.OnHealChanged += UpdateHeal;
        character.OnStateChanged += UpdateStates;

        UpdateHeal();
        UpdateMana();

        characterImage.sprite = character.BattleImage;
        nameText.text = character.Name;

        SetDead(false);
        MarkTarget(false);
        ChangeAct(BattleTurnData.TurnAction.None);
        UpdateStates();

        initialized = true;
    }

    public void ChangeAct(BattleTurnData.TurnAction action)
    {
        actImage.enabled = action != BattleTurnData.TurnAction.None;

        actImage.sprite = Battle.data.GetActionIcon(action);
    }

    public void MarkTarget(bool mark)
    {
        istargetobj.SetActive(mark);
    }

    public void SetDead(bool dead)
    {
        isDead = dead;

        UpdateStates();

        isdead.SetActive(dead);
    }

    private void UpdateHeal()
    {
        healBar.SetValue((float)Character.Heal / (float)Character.MaxHeal);
    }

    private void UpdateMana()
    {
        manaBar.SetValue((float)Character.Mana / (float)Character.MaxMana);
    }

    public void UpdateStates(RPGEntityState state)
    {
        iconList.UpdateIcons(Character.States.Select(i => i.Icon).ToArray());
    }

    public void UpdateStates()
    {
        iconList.UpdateIcons(Character.States.Select(i => i.Icon).ToArray());
    }

    private void OnDestroy()
    {
        Dispose();
    }

    public void Dispose()
    {
        if (initialized)
        {
            Character.OnManaChanged -= UpdateMana;
            Character.OnHealChanged -= UpdateHeal;
            Character.OnAllStatesChanged -= UpdateStates;

            Character = null;

            iconList.Dispose();

            initialized = false;
        }
    }
}
