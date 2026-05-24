using RPGF.Domain.Interfaces;
using RPGF.GUI;
using RPGF.RPG;
using TMPro;
using UnityEngine;

namespace GlackSaga.GUI.TitleMenu.Gear
{
    public class GearSlotGUIBlock : GUISelectableBlock, ISetData<RPGCharacter>
    {
        [Header("LINKS:")]
        [SerializeField]
        private TextMeshProUGUI itemDescriptionText;
        [SerializeField]
        private GearSelectionGUIBlock gearSelectionGUIBlock;
        [Space]
        [SerializeField]
        private GearStatDisplay maxHpDisplay;
        [SerializeField]
        private GearStatDisplay maxMpDisplay;
        [SerializeField]
        private GearStatDisplay damageDisplay;
        [SerializeField]
        private GearStatDisplay defenseDisplay;
        [SerializeField]
        private GearStatDisplay agilityDisplay;
        [SerializeField]
        private GearStatDisplay luckDisplay;

        private RPGCharacter character;

        protected override void OnActivate()
        {
            base.OnActivate();
        }

        protected override void OnSelectionChanged()
        {
            var element = Elements[CurrentIndex] as GearSlotItem;
            if (element != null)
            {
                SetItemPreview(character.GetWerableByType(element.UsedType));
            }
        }

        protected override void OnChoiced(int index)
        {
            if (Elements[index] is GearSlotItem gearSlotItem)
            {
                gearSelectionGUIBlock.SetData(character, gearSlotItem.UsedType);
                gearSelectionGUIBlock.RefreshSlots();
                Manager.NextBlock(gearSelectionGUIBlock);
            }
        }

        public void UpdateSlots()
        {
            foreach (var slot in Elements)
            {
                if (slot is GearSlotItem gearSlotItem)
                {
                    gearSlotItem.SetData(character.GetWerableByType(gearSlotItem.UsedType));
                }
            }
        }

        public void SetData(RPGCharacter value)
        {
            character = value;

            UpdateStats();
            UpdateSlots();
        }

        public void UpdateStats()
        {
            maxHpDisplay.SetStat($"{GetLocale("SYS_MAX_HEAL_S")}: {character.MaxHeal}");
            maxMpDisplay.SetStat($"{GetLocale("SYS_MAX_MANA_S")}: {character.MaxMana}");
            damageDisplay.SetStat($"{GetLocale("SYS_DMG_S")}: {character.Damage}");
            defenseDisplay.SetStat($"{GetLocale("SYS_DEF_S")}: {character.Defense}");
            agilityDisplay.SetStat($"{GetLocale("SYS_AGI_S")}: {character.Agility}");
            luckDisplay.SetStat($"{GetLocale("SYS_LUCK_S")}: {character.Luck}");
        }

        public void SetItemPreview(RPGWerable value)
        {
            if (value == null)
            {
                ClearItemPreview();
                return;
            }
            itemDescriptionText.text = GetLocale(value.GetLocaleDesciptionTag(), value.Description);

            var oldValue = character.GetWerableByType(value.UsedOn);
            if (oldValue != null)
            {
                maxHpDisplay.SetStatChange(value.Heal - oldValue.Heal);
                maxMpDisplay.SetStatChange(value.Mana - oldValue.Mana);
                damageDisplay.SetStatChange(value.Damage - oldValue.Damage);
                defenseDisplay.SetStatChange(value.Defense - oldValue.Defense);
                agilityDisplay.SetStatChange(value.Agility - oldValue.Agility);
                luckDisplay.SetStatChange(value.Luck - oldValue.Luck);
            }
            else
            {
                maxHpDisplay.SetStatChange(value.Heal);
                maxMpDisplay.SetStatChange(value.Mana);
                damageDisplay.SetStatChange(value.Damage);
                defenseDisplay.SetStatChange(value.Defense);
                agilityDisplay.SetStatChange(value.Agility);
                luckDisplay.SetStatChange(value.Luck);
            }
        }

        public void ClearItemPreview()
        {
            itemDescriptionText.text = string.Empty;

            maxHpDisplay.SetStatChange(0);
            maxMpDisplay.SetStatChange(0);
            damageDisplay.SetStatChange(0);
            defenseDisplay.SetStatChange(0);
            agilityDisplay.SetStatChange(0);
            luckDisplay.SetStatChange(0);
        }

        public void SetUnequipPreview(RPGWerable.UsedType type)
        {
            itemDescriptionText.text = string.Empty;

            var equipped = character.GetWerableByType(type);
            if (equipped == null)
            {
                maxHpDisplay.SetStatChange(0);
                maxMpDisplay.SetStatChange(0);
                damageDisplay.SetStatChange(0);
                defenseDisplay.SetStatChange(0);
                agilityDisplay.SetStatChange(0);
                luckDisplay.SetStatChange(0);
                return;
            }

            maxHpDisplay.SetStatChange(-equipped.Heal);
            maxMpDisplay.SetStatChange(-equipped.Mana);
            damageDisplay.SetStatChange(-equipped.Damage);
            defenseDisplay.SetStatChange(-equipped.Defense);
            agilityDisplay.SetStatChange(-equipped.Agility);
            luckDisplay.SetStatChange(-equipped.Luck);
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            ClearItemPreview();
        }
    }
}
