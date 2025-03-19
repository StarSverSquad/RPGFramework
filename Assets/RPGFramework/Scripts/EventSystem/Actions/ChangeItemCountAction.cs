using RPGF.RPG;
using System;
using System.Collections;

public class ChangeItemCountAction : GraphActionBase
{
    public RPGCollectable Item;

    public int Count;

    public bool IsSet;

    private InventoryManager Inventory => GameManager.Instance.Inventory;

    public ChangeItemCountAction() : base("ChangeItemCount")
    {
        Item = null;

        Count = 0;

        IsSet = false;
    }

    public override IEnumerator ActionCoroutine()
    {
        if (IsSet)
            Inventory.SetItemCount(Item, Count);
        else
            Inventory.AddToItemCount(Item, Count);

        yield break;
    }

    public override string GetHeader()
    {
        return "Измененить количество предмета";
    }
}