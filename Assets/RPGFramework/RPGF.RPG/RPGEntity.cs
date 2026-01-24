using RPGF.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPGF.RPG
{
    public class RPGEntity : RPGBase, ICloneable<RPGEntity>
    {
        [Header("Настройки сущности")]
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

        public event Action<int, RPGEntity> OnDamage;

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

        #region [ДЛЯ СОСТОЯНИЙ]

        /// <summary>
        /// Добовляет состоаяние
        /// </summary>
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
        /// <summary>
        /// Удаляет состояние
        /// </summary>
        public virtual void RemoveState(RPGEntityState state)
        {
            if (!HasState(state))
                return;

            stateInstances.Remove(GetStateInstance(state));

            UpdateStats();

            OnStateRemoved?.Invoke(state);
            OnStateChanged?.Invoke(state);
        }
        /// <summary>
        /// Удаляет все состояния
        /// </summary>
        public virtual void RemoveAllStates()
        {
            stateInstances.Clear();

            UpdateStats();

            OnAllStatesChanged?.Invoke();
        }
        /// <summary>
        /// Удаляет только те состояния которые не могут существовать вне битвы
        /// </summary>
        public virtual void RemoveNonBattleStates()
        {
            RPGEntityState[] states = States.Where(i => i.OnlyForBattle).ToArray();

            foreach (RPGEntityState state in states)
                RemoveState(state);

            UpdateStats();
        }
        /// <summary>
        /// Обнавляет выбранное состояние
        /// </summary>
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
        /// <summary>
        /// Обнавляет все состояния
        /// </summary>
        public virtual void UpdateAllStates()
        {
            foreach (var state in States)
                UpdateState(state);

            OnAllStatesChanged?.Invoke();
        }
        /// <summary>
        /// Проверяет наличие состояния
        /// </summary>
        public virtual bool HasState(RPGEntityState state) => States.Any(i => i.Tag == state.Tag);
        /// <summary>
        /// Создаёт экзмпляр состояния
        /// </summary>
        public virtual RPGEntityStateInstance GetStateInstance(RPGEntityState state) => stateInstances.FirstOrDefault(i => i.Original.Tag == state.Tag);

        #endregion

        public virtual int GiveDamage(RPGEntity who, float damageModifier = 1)
        {
            int resultDamage = CalculateDamage(who, damageModifier);

            if (resultDamage <= 0)
                return 0;

            Heal -= resultDamage;

            OnDamage?.Invoke(resultDamage, who);

            return resultDamage;
        }
        public virtual int GiveDamage(int damage)
        {
            int resultDamage = CalculateDamage(damage);

            if (resultDamage <= 0)
                return 0;

            Heal -= resultDamage;

            OnDamage?.Invoke(resultDamage, null);

            return resultDamage;
        }

        /// <summary>
        /// Расчитвывет персональный урон для этого энтити по формуле
        /// </summary>
        public virtual int CalculateDamage(int damage)
        {
            var result = Mathf.RoundToInt(damage) - Mathf.RoundToInt(Defence * .5f);

            result = Mathf.RoundToInt(UnityEngine.Random.Range(result * 0.75f, result * 1.25f));

            return result;
        }
        /// <summary>
        /// Расчитвывет персональный урон для этого энтити по формуле
        /// </summary>
        public virtual int CalculateDamage(RPGEntity who, float damageModifier = 1)
        {
            var result = Mathf.RoundToInt(who.Damage * damageModifier) - Mathf.RoundToInt(Defence * .5f);

            result = Mathf.RoundToInt(UnityEngine.Random.Range(result * 0.75f, result * 1.25f));

            return result;
        }

        public virtual RPGEntity Clone()
        {
            return Instantiate(this);
        }
    }
}
