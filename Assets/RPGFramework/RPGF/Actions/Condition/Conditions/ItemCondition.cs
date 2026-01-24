using RPGF;
using RPGF.Core.Inventory;
using RPGF.Domain.DI;
using RPGF.RPG;
using UnityEngine;

namespace RPGF.Actions.Condition
{
    [UseCondition("По предмету")]
    public class ItemCondition : ConditionBase
    {
        [Inject]
        private readonly InventoryService _inventory;

        public RPGCollectable Value;

        public int Count;

        public ConditionOperation Operation;

        public ItemCondition()
        {
            Value = null;

            Count = 0;

            Operation = ConditionOperation.Equals;
        }

        public override bool Invoke()
        {
            if (Value == null)
            {
                Debug.LogError($"HAVE_ITEM_CONDITION: Предмет не указан");

                return false;
            }

            if (!_inventory.HasItemSlot(Value))
                return false;

            InventorySlotData slot = _inventory.GetSlotByItemTag(Value.Tag);

            return Operation switch
            {
                ConditionOperation.Equals => slot.Count == Count,
                ConditionOperation.NotEquals => slot.Count != Count,
                ConditionOperation.More => slot.Count > Count,
                ConditionOperation.Less => slot.Count < Count,
                ConditionOperation.MoreOrEquals => slot.Count >= Count,
                ConditionOperation.LessOrEquals => slot.Count <= Count,
                _ => false,
            };
        }
    }
}