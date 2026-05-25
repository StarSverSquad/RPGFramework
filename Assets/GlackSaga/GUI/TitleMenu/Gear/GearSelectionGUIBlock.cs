using System.Collections.Generic;
using System.Linq;
using GlackSaga.GUI.TitleMenu;
using RPGF.Core.Inventory;
using RPGF.GUI.Interfaces;
using RPGF.RPG;
using UnityEngine;

namespace GlackSaga.GUI.TitleMenu.Gear
{
    public class GearSelectionGUIBlock : InventoryPaginatedGUIBlock
    {
        [Header("LINKS:")]
        [SerializeField]
        private GearSlotGUIBlock gearSlotGUIBlock;
        [SerializeField]
        private GameObject upArrow;
        [SerializeField]
        private GameObject downArrow;
        [SerializeField]
        private AudioSource equipSound;
        [SerializeField]
        private AudioSource unequipSound;

        private RPGCharacter character;
        private RPGWerable.UsedType usedType;

        public void SetData(RPGCharacter character, RPGWerable.UsedType usedType)
        {
            this.character = character;
            this.usedType = usedType;
        }

        public override void Initialize(IGUIManager manager)
        {
            base.Initialize(manager);
        }

        protected override IEnumerable<InventorySlotData> BuildItems()
        {
            yield return null;

            foreach (var slot in FilterSlots(Inventory.Slots))
            {
                yield return slot;
            }
        }

        protected override void OnActivate()
        {
            RefreshItems();

            base.OnActivate();

            if (HasItems)
            {
                ChangeSelect(0);
            }
            else
            {
                gearSlotGUIBlock.ClearItemPreview();
            }
        }

        protected override void OnChoiced(int index)
        {
            if (!HasItems)
            {
                return;
            }

            var slot = GetItemAt(ToAbsoluteIndex(index));

            if (slot == null)
            {
                UnequipCurrent();
                Preview();
                return;
            }

            var newItem = slot.Item as RPGWerable;
            var oldItem = character.GetWerableByType(usedType);

            if (oldItem != null)
            {
                Inventory.AddToItemCount(oldItem, 1);
            }

            character.SetWerableByType(usedType, newItem);
            character.UpdateStats();

            Inventory.AddToItemCount(newItem, -1);

            gearSlotGUIBlock.UpdateSlots();
            gearSlotGUIBlock.UpdateStats();
            gearSlotGUIBlock.SetItemPreview(newItem);

            equipSound.Play();

            Preview();
        }

        private void UnequipCurrent()
        {
            unequipSound.Play();
            gearSlotGUIBlock.ClearItemPreview();

            var oldItem = character.GetWerableByType(usedType);
            if (oldItem == null)
            {
                return;
            }

            Inventory.AddToItemCount(oldItem, 1);
            character.SetWerableByType(usedType, null);
            character.UpdateStats();

            gearSlotGUIBlock.UpdateSlots();
            gearSlotGUIBlock.UpdateStats();
        }

        protected override void OnCanceled()
        {
            var equipped = character.GetWerableByType(usedType);
            if (equipped != null)
            {
                gearSlotGUIBlock.SetItemPreview(equipped);
            }
            else
            {
                gearSlotGUIBlock.ClearItemPreview();
            }
        }

        protected override IEnumerable<InventorySlotData> FilterSlots(IEnumerable<InventorySlotData> slots)
        {
            return slots.Where(MatchesSlot);
        }

        protected override void BindElement(int elementIndex, InventorySlotData slot)
        {
            if (Elements[elementIndex] is GearSelectionItem gearSelectionItem)
            {
                gearSelectionItem.SetData(slot);
            }
            Elements[elementIndex].gameObject.SetActive(true);
        }

        protected override void HideElement(int elementIndex)
        {
            Elements[elementIndex].gameObject.SetActive(false);
        }

        protected override void UpdatePaginationArrows(int page, int maxPage)
        {
            upArrow.SetActive(page > 0);
            downArrow.SetActive(page < maxPage);
        }

        protected override void OnItemSelected(InventorySlotData slot)
        {
            if (slot == null)
            {
                gearSlotGUIBlock.SetUnequipPreview(usedType);
                return;
            }

            gearSlotGUIBlock.SetItemPreview(slot.Item as RPGWerable);
        }

        private bool MatchesSlot(InventorySlotData slot)
        {
            if (slot.Item is not RPGWerable wearable || wearable.UsedOn != usedType)
            {
                return false;
            }

            if (usedType == RPGWerable.UsedType.Weapon && slot.Item is not RPGWeapon)
            {
                return false;
            }

            return wearable.RequireClasses.Count == 0
                || wearable.RequireClasses.Contains(character.Class);
        }
    }
}
