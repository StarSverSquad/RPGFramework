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
    private Animator iconsAnimator;

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
    private Sprite[] actIcons = new Sprite[4];

    [SerializeField]
    private LineBar healBar;
    [SerializeField]
    private LineBar manaBar;

    [SerializeField]
    private BattleCharacterInfo character;
    public BattleCharacterInfo Character => character;

    [SerializeField]
    private bool isDead = false;
    public bool IsDead => isDead;

    private bool initialized = false;

    public void Initialize(BattleCharacterInfo ch)
    {
        character = ch;

        ch.OnManaChanged += UpdateMana;
        ch.OnHealChanged += UpdateHeal;
        ch.StatesChanged += UpdateStates;

        UpdateHeal();
        UpdateMana();

        characterImage.sprite = ch.Character.Icon;
        nameText.text = ch.Entity.Name;

        SetDead(false);
        MarkTarget(false);
        ChangeAct(BattleCharacterAction.None);
        UpdateStates();

        initialized = true;
    }


    /// <summary>
    /// Смена значка действия
    /// </summary>
    public void ChangeAct(BattleCharacterAction action)
    {
        actImage.enabled = true;


        switch (action)
        {
            case BattleCharacterAction.None:
                actImage.enabled = false;
                break;
            case BattleCharacterAction.Fight:
                actImage.sprite = actIcons[0];
                break;
            case BattleCharacterAction.Act:
                actImage.sprite = actIcons[1];
                break;
            case BattleCharacterAction.Item:
                actImage.sprite = actIcons[2];
                break;
            case BattleCharacterAction.Defence:
                actImage.sprite = actIcons[3];
                break;
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
        healBar.SetValue((float)character.Entity.Heal / (float)character.Entity.MaxHeal);
    }

    private void UpdateMana()
    {
        manaBar.SetValue((float)character.Entity.Mana / (float)character.Entity.MaxMana);
    }

    /// <summary>
    /// Обноаляет значки состояний персонажа
    /// </summary>
    /// <param name="states">Новые состояния</param>
    public void UpdateStates(params RPGEntityState[] states)
    {
        iconList.UpdateIcons(states.Select(i => i.Icon).ToArray());

        SetStatesVisibility(iconList.HasIcons);
    }
    /// <summary>
    /// Обноаляет значки состояний персонажа
    /// </summary>
    public void UpdateStates()
    {
        iconList.UpdateIcons(character.States.Select(i => i.rpg.Icon).ToArray());

        SetStatesVisibility(iconList.HasIcons);
    }

    private void SetStatesVisibility(bool visibility)
    {
        iconsAnimator.SetBool("IsShow", visibility);
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
            character.StatesChanged -= UpdateStates;

            character = null;

            iconList.Dispose();

            initialized = false;
        }
    }
}
