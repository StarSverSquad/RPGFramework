using System.Collections.Generic;
using System.Linq;
using RPGF.Core.Inventory;
using RPGF.Domain.DI;
using RPGF.Domain.Interfaces;
using RPGF.GUI;
using UnityEngine;

namespace GlackSaga.GUI.TitleMenu
{
    public abstract class InventoryPaginatedGUIBlock : GUISelectableBlock
    {
        [Inject]
        private readonly InventoryService _inventory = null!;

        protected List<InventorySlotData> SelectedSlots { get; } = new();

        public int Page { get; protected set; }
        public virtual int PageSize => Elements.Count;
        public int MaxPage => SelectedSlots.Count == 0 || PageSize == 0
            ? 0
            : Mathf.CeilToInt((float)SelectedSlots.Count / PageSize) - 1;

        protected int AbsoluteIndex => CurrentIndex + (Page * PageSize);

        protected InventorySlotData GetSlotAt(int listIndex) => SelectedSlots[listIndex];

        protected InventorySlotData GetCurrentSlot() => SelectedSlots[AbsoluteIndex];

        public bool HasSlots => SelectedSlots.Count > 0;

        protected InventoryService Inventory => _inventory;

        public void RefreshSlots()
        {
            SelectedSlots.Clear();
            SelectedSlots.AddRange(FilterSlots(_inventory.Slots));

            OnSlotsRefreshed();
            SetPage(0);
        }

        protected void SetPage(int page)
        {
            Page = page;

            for (int i = 0; i < PageSize; i++)
            {
                var slotIndex = i + (page * PageSize);
                if (slotIndex >= SelectedSlots.Count)
                {
                    HideElement(i);
                    continue;
                }

                BindElement(i, SelectedSlots[slotIndex]);
            }

            UpdatePaginationArrows(page, MaxPage);
        }

        protected override void ChangeSelect(int newIndex)
        {
            if (PageSize == 0 || !HasSlots)
            {
                return;
            }

            if (Page > 0 && newIndex < 0)
            {
                SetPage(Page - 1);
                base.ChangeSelect(PageSize - 1);
            }
            else if (Page < MaxPage && newIndex >= PageSize)
            {
                SetPage(Page + 1);
                base.ChangeSelect(0);
            }
            else
            {
                var absoluteIndex = newIndex + (Page * PageSize);
                if (absoluteIndex >= SelectedSlots.Count || absoluteIndex < 0)
                {
                    return;
                }

                base.ChangeSelect(newIndex);
            }

            OnSlotSelected(GetCurrentSlot());
        }

        protected override void OnDiativate()
        {
            base.OnDiativate();

            Page = 0;
            SelectedSlots.Clear();
        }

        protected abstract IEnumerable<InventorySlotData> FilterSlots(IEnumerable<InventorySlotData> slots);

        protected abstract void BindElement(int elementIndex, InventorySlotData slot);

        protected abstract void HideElement(int elementIndex);

        protected abstract void UpdatePaginationArrows(int page, int maxPage);

        protected virtual void OnSlotsRefreshed() { }

        protected virtual void OnSlotSelected(InventorySlotData slot) { }

        protected void BindElementWithInterface(int elementIndex, InventorySlotData slot)
        {
            if (Elements[elementIndex] is ISetData<InventorySlotData> setData)
            {
                setData.SetData(slot);
            }

            Elements[elementIndex].gameObject.SetActive(true);
        }

        protected void HideElementAt(int elementIndex)
        {
            Elements[elementIndex].gameObject.SetActive(false);
        }
    }
}
