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

    [Tooltip("Fight, Act, Item, Defence")]
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

        characterImage.sprite = character.Icon;
        nameText.text = character.Name;

        SetDead(false);
        MarkTarget(false);
        ChangeAct(TurnAction.None);
        UpdateStates();

        initialized = true;
    }


    /// <summary>
    /// Смена значка действия
    /// </summary>
    public void ChangeAct(TurnAction action)
    {
        actImage.enabled = true;


        switch (action)
        {
            case TurnAction.Fight:
                actImage.sprite = actIcons[0];
                break;
            case TurnAction.Act:
                actImage.sprite = actIcons[1];
                break;
            case TurnAction.Item:
                actImage.sprite = actIcons[2];
                break;
            case TurnAction.Defence:
                actImage.sprite = actIcons[3];
                break;
            case TurnAction.Spell:
                actImage.sprite = actIcons[4];
                break;
            case TurnAction.None:
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
