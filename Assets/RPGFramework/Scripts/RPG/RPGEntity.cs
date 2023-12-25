using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RPGEntity : ScriptableObject
{
    public string Name;

    [Multiline(3)]
    public string Description;

    [Header("Íàñòðîéêè ñóùíîñòè")]
    public int DefaultHeal;
    public int DefaultMana;

    public int DefaultDamage;
    public int DefaultDefence;
    public int DefaultAgility;
    public int DefaultLuck;

    private int heal;
    public int Heal
    {
        get => heal; 

        set 
        {
            heal = Mathf.Clamp(value, 0, MaxHeal);

            OnHealChanged?.Invoke();
        }
    }

    private int mana;
    public int Mana
    {
        get => mana;

        set
        {
            mana = Mathf.Clamp(value, 0, MaxMana);

            OnManaChanged?.Invoke();
        }
    }

    public int MaxHeal { get; set; }
    public int MaxMana { get; set; }

    public int Damage { get; set; }
    public int Defence { get; set; }
    public int Agility { get; set; }
    public int Luck { get; set; }
    
    private List<RPGEntityStateInstance> stateInstances = new List<RPGEntityStateInstance>();
    public RPGEntityStateInstance[] StateInstances => stateInstances.ToArray();
    public RPGEntityState[] States => stateInstances.Select(i => i.Original).ToArray();

    public event Action OnHealChanged;
    public event Action OnManaChanged;

    public event Action OnAllStatesChanged;
    public event Action<RPGEntityState> OnStateChanged;
    public event Action<RPGEntityState> OnStateUpdated;
    public event Action<RPGEntityState> OnStateRemoved;
    public event Action<RPGEntityState> OnStateAdded;

    public virtual void InitializeEntity()
    {
        RemoveAllStates();

        UpdateStats();

        Heal = MaxHeal; Mana = MaxMana;
    }

    public virtual void UpdateStats()
    {
        Damage = DefaultDamage;
        Defence = DefaultDefence;
        Agility = DefaultAgility;
        Luck = DefaultLuck;

        MaxHeal = DefaultHeal;
        MaxMana = DefaultMana;

        foreach (var state in States)
        {
            Damage += state.AddDamage;
            Defence += state.AddDefence;
            Agility += state.AddAgility;
            Luck += state.AddLuck;
        }
    }

    #region [ÄËß ÑÎÑÒÎßÍÈÉ]

    public virtual void AddState(RPGEntityState state)
    {
        if (HasState(state))
        {
            GetStateInstance(state).TurnsLeft = state.TurnCount;
            return;
        }

        stateInstances.Add(new RPGEntityStateInstance(state));

        UpdateStats();

        OnStateAdded?.Invoke(state);
        OnStateChanged?.Invoke(state);
    }

    public virtual void RemoveState(RPGEntityState state)
    {
        if (!HasState(state))
            return;

        stateInstances.Remove(GetStateInstance(state));

        UpdateStats();

        OnStateRemoved?.Invoke(state);
        OnStateChanged?.Invoke(state);
    }

    public virtual void RemoveAllStates()
    {
        stateInstances.Clear();

        UpdateStats();

        OnAllStatesChanged?.Invoke();
    }

    public virtual void RemoveNonBattleStates()
    {
        RPGEntityState[] states = States.Where(i => i.OnlyForBattle).ToArray();

        foreach (RPGEntityState state in states)
            RemoveState(state);

        UpdateStats();
    }

    public virtual void UpdateState(RPGEntityState state)
    {
        if (!HasState(state))
            return;

        Heal = Mathf.Clamp(Heal + state.AddHeal, 1, MaxHeal);
        Mana = Mathf.Clamp(Mana + state.AddMana, 1, MaxMana);

        RPGEntityStateInstance instance = GetStateInstance(state);

        instance.TurnsLeft--;

        OnStateUpdated?.Invoke(state);

        if (instance.TurnsLeft <= 0)
            RemoveState(state);
    }

    public virtual void UpdateAllStates()
    {
        foreach (var state in States)
            UpdateState(state);

        OnAllStatesChanged?.Invoke();
    }

    public virtual bool HasState(RPGEntityState state) => States.Any(i => i.Name == state.Name);

    public virtual RPGEntityStateInstance GetStateInstance(RPGEntityState state) => stateInstances.FirstOrDefault(i => i.Original.Name == state.Name);

    #endregion

    public virtual int GiveDamage(RPGEntity who, float DamageModifier = 1, bool dontHurt = false)
    {
        int resultDamage = Mathf.RoundToInt(who.Damage * DamageModifier) - Mathf.RoundToInt(Defence / .5f);

        resultDamage = Mathf.RoundToInt(UnityEngine.Random.Range(resultDamage * 0.75f, resultDamage * 1.25f));

        if (resultDamage <= 0)
            return 0;

        if (!dontHurt)
            Heal -= resultDamage;

        return resultDamage;
    }
    public virtual int GiveDamage(int damage, bool dontHurt = false)
    {
        int resultDamage = Mathf.RoundToInt(damage) - Mathf.RoundToInt(Defence / .5f);

        resultDamage = Mathf.RoundToInt(UnityEngine.Random.Range(resultDamage * 0.75f, resultDamage * 1.25f));

        if (resultDamage <= 0)
            return 0;

        if (!dontHurt)
            Heal -= resultDamage;

        return resultDamage;
    }
}
