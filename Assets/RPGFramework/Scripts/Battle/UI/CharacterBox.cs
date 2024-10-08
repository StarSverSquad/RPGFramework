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
    private BattleCharacterInfo character;
    public BattleCharacterInfo Character => character;

    [SerializeField]
    private bool isDead = false;
    public bool IsDead => isDead;

    private bool initialized = false;

    public void Initialize(BattleCharacterInfo ch)
    {
        character = ch;

        ch.Entity.OnManaChanged += UpdateMana;
        ch.Entity.OnHealChanged += UpdateHeal;
        ch.Entity.OnStateChanged += UpdateStates;

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
    /// ����� ������ ��������
    /// </summary>
    public void ChangeAct(BattleCharacterAction action)
    {
        actImage.enabled = true;


        switch (action)
        {
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
            case BattleCharacterAction.Spell:
                actImage.sprite = actIcons[4];
                break;
            case BattleCharacterAction.None:
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
    /// ��������� ������ ��������� ���������
    /// </summary>
    /// <param name="states">����� ���������</param>
    public void UpdateStates(RPGEntityState state)
    {
        iconList.UpdateIcons(Character.States.Select(i => i.Icon).ToArray());

        //SetStatesVisibility(iconList.HasIcons);
    }

    /// <summary>
    /// ��������� ������ ��������� ���������
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
            character.Entity.OnManaChanged -= UpdateMana;
            character.Entity.OnHealChanged -= UpdateHeal;
            character.Entity.OnAllStatesChanged -= UpdateStates;

            character = null;

            iconList.Dispose();

            initialized = false;
        }
    }
}
