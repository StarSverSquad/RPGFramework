using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBox : MonoBehaviour, IDisposable
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

    [Tooltip("Attack, Act, Item, Flee")]
    [SerializeField]
    private Sprite[] actIcons = new Sprite[5];

    [SerializeField]
    private LineBar healBar;
    [SerializeField]
    private LineBar manaBar;

    [SerializeField]
    private RPGCharacter character;
    public RPGCharacter Character => character;

    [SerializeField]
    private bool isDead = false;
    public bool IsDead => isDead;

    private bool initialized = false;

    public void Initialize(RPGCharacter character)
    {
        this.character = character;

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


    /// <summary>
    /// Смена значка действия
    /// </summary>
    public void ChangeAct(BattleTurnData.TurnAction action)
    {
        actImage.enabled = true;


        switch (action)
        {
            case BattleTurnData.TurnAction.Attack:
                actImage.sprite = actIcons[0];
                break;
            case BattleTurnData.TurnAction.Act:
                actImage.sprite = actIcons[1];
                break;
            case BattleTurnData.TurnAction.Item:
                actImage.sprite = actIcons[2];
                break;
            case BattleTurnData.TurnAction.Flee:
                actImage.sprite = actIcons[3];
                break;
            case BattleTurnData.TurnAction.Defence:
                actImage.sprite = actIcons[3];
                break;
            case BattleTurnData.TurnAction.Ability:
                actImage.sprite = actIcons[4];
                break;
            case BattleTurnData.TurnAction.None:
            default:
            actImage.enabled = false;
                break;
        }
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
        healBar.SetValue((float)character.Heal / (float)character.MaxHeal);
    }

    private void UpdateMana()
    {
        manaBar.SetValue((float)character.Mana / (float)character.MaxMana);
    }

    /// <summary>
    /// Обноаляет значки состояний персонажа
    /// </summary>
    /// <param name="states">Новые состояния</param>
    public void UpdateStates(RPGEntityState state)
    {
        iconList.UpdateIcons(Character.States.Select(i => i.Icon).ToArray());

        //SetStatesVisibility(iconList.HasIcons);
    }

    /// <summary>
    /// Обноаляет значки состояний персонажа
    /// </summary>
    public void UpdateStates()
    {
        iconList.UpdateIcons(character.States.Select(i => i.Icon).ToArray());

        //SetStatesVisibility(iconList.HasIcons);
    }

    private void OnDestroy()
    {
        Dispose();
    }

    public void Dispose()
    {
        if (initialized)
        {
            character.OnManaChanged -= UpdateMana;
            character.OnHealChanged -= UpdateHeal;
            character.OnAllStatesChanged -= UpdateStates;

            character = null;

            iconList.Dispose();

            initialized = false;
        }
    }
}
