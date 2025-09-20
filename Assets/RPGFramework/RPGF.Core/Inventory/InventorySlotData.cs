using RPGF.RPG;
using System;

namespace RPGF.Core.Inventory
{
    [Serializable]
    public class InventorySlotData
    {
        public RPGCollectable Item;

        public int Count;
    }
}