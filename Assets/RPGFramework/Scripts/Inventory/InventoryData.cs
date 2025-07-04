using System.Collections.Generic;

namespace RPGF.Inventory
{
    public class InventoryData
    {
        public List<InventorySlotData> Slots;

        public InventoryData()
        {
            Slots = new List<InventorySlotData>();
        }
    }
}
