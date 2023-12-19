using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEditor.Progress;

[Serializable]
public class BattleEntityInfo
{
    public delegate void StateUpdatedAction(EntityState state, int changedHeal, int changedMana);
    public delegate void AllStatesUpdatedAction(int totalHeal, int totalMana);

    public class EntityState
    {
        public RPGEntityState rpg;
        public int turnsCount;
    }

    public RPGEntity Entity { get; set; }

    public int Heal
    {
        get => Entity.Heal;
        set
        {
            Entity.Heal = value;
            OnHealChanged?.Invoke();
        }
    }
    public int Mana
    {
        get => Entity.Mana;
        set
        {
            Entity.Mana = value;
            OnManaChanged?.Invoke();
        }
    }

    public List<EntityState> States { get; set; }

    public BattleEntityInfo(RPGEntity entity)
    {
        Entity = entity;
        States = new List<EntityState>();
    }

    public event Action StatesChanged;
    public event Action OnHealChanged;
    public event Action OnManaChanged;

    public void AddState(RPGEntityState state)
    {
        EntityState st = States.FirstOrDefault(i => i.rpg == state);

        if (st != null)
        {
            st.turnsCount = 0;
            return;
        }

        EntityState entityState = new EntityState()
        {
            rpg = state,
            turnsCount = 0
        };

        Entity.Damage += state.AddDamage;
        Entity.Defence += state.AddDefence;
        Entity.Agility += state.AddAgility;

        States.Add(entityState);

        InvokeStatesChangedCallback();
    }

    public void RemoveState(RPGEntityState state)
    {
        EntityState st = States.FirstOrDefault(i => i.rpg == state);

        if (st == null)
            return;

        Entity.Damage -= state.AddDamage;
        Entity.Defence -= state.AddDefence;
        Entity.Agility -= state.AddAgility;

        States.Remove(st);

        InvokeStatesChangedCallback();
    }

    public void RemoveAllStates()
    {
        foreach (var state in States)
        {
            Entity.Damage -= state.rpg.AddDamage;
            Entity.Defence -= state.rpg.AddDefence;
            Entity.Agility -= state.rpg.AddAgility;
        }
        States.Clear();

        StatesChanged?.Invoke();
    }

    public void UpdateAllStates()
    {
        for (int i = 0, oldcount = States.Count; i < States.Count; i++)
        {
            UpdateState(States[i].rpg);

            if (oldcount != States.Count)
            {
                i--;
                oldcount = States.Count;
            }
        }   
    }

    public virtual void UpdateState(RPGEntityState state)
    {
        EntityState st = States.FirstOrDefault(i => i.rpg == state);

        if (st == null)
            return;

        Heal += state.AddHeal;
        Mana += state.AddMana;

        st.turnsCount++;

        if (st.turnsCount >= state.TurnCount)
            RemoveState(state);
    }

    public virtual void Damage(int damage)
    {
        Entity.GiveDamage(damage);

        OnHealChanged?.Invoke();
    }

    protected void InvokeOnDamageCallback() => OnHealChanged?.Invoke();
    protected void InvokeOnManaChangedCallback() => OnManaChanged?.Invoke();
    protected void InvokeStatesChangedCallback() => StatesChanged?.Invoke();
}
