using RPGF.RPG;
using UnityEngine;

public class ItemCondition : ConditionBase
{
    public RPGCollectable Value;

    public int Count;

    public ConditionOperation Operation;

    private InventoryManager Inventory => GameManager.Instance.Inventory;

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

        if (!Inventory.HasItemSlot(Value))
            return false;

        InventorySlot slot = Inventory.GetSlotByItemTag(Value.Tag);

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

    public override string GetLabel()
    {
        return "По предмету";
    }
}