using System.Collections.Generic;
using System.Linq;
using RPGF.Core.Inventory;
using RPGF.Domain.DI;
using RPGF.Domain.Interfaces;
using RPGF.GUI;
using UnityEngine;

namespace GlackSaga.GUI.TitleMenu
{
    public abstract class InventoryPaginatedGUIBlock : PaginatedSelectableGUIBlock<InventorySlotData>
    {
        [Inject]
        private readonly InventoryService _inventory = null!;

        protected InventoryService Inventory => _inventory;

        protected List<InventorySlotData> SelectedSlots => SelectedItems;

        protected InventorySlotData GetSlotAt(int listIndex) => GetItemAt(listIndex);

        protected override IEnumerable<InventorySlotData> BuildItems() => FilterSlots(_inventory.Slots);

        protected abstract IEnumerable<InventorySlotData> FilterSlots(IEnumerable<InventorySlotData> slots);

        protected void BindElementWithInterface(int elementIndex, InventorySlotData slot)
        {
            if (Elements[elementIndex] is ISetData<InventorySlotData> setData)
            {
                setData.SetData(slot);
            }

            ShowElement(elementIndex);
        }
    }
}
