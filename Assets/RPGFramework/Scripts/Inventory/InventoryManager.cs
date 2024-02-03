using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IEnumerable<InventorySlot>, IDisposable
{
    private List<InventorySlot> slots = new List<InventorySlot>();
    public InventorySlot[] Slots => slots.ToArray();

    public InventorySlot this[string itemName]
    {
        get => GetSlotByItemName(itemName);
    }
    public InventorySlot this[RPGCollectable item]
    {
        get => slots.First(i => i.Item == item);
    }

    /// <summary>
    /// Возвращает список всех потребляемых предметов
    /// </summary>
    public List<RPGConsumed> GetConsumeds()
    {
        return slots.Where(i => i.Item is RPGConsumed).Select(i => i.Item as RPGConsumed).ToList();
    }
    /// <summary>
    /// Возвращает список всех надеваемый предметов
    /// </summary>
    public List<RPGWerable> GetWerables()
    {
        return slots.Where(i => i.Item is RPGWerable).Select(i => i.Item as RPGWerable).ToList();
    }

    public InventorySlot GetSlotByItemName(string name)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.Item.Tag == name)
                return slot;
        }

        return null;
    }

    /// <summary>
    /// <br>Создаёт пустой слот</br>
    /// <br>[Не рекомендуеться]</br>
    /// <br>Используйте AddToItemCount, SetItemCount</br>
    /// </summary>
    public InventorySlot CreateSlot(RPGCollectable slotfor)
    {
        InventorySlot slot = new InventorySlot()
        {
            Item = slotfor,
            Count = 0
        };

        slots.Add(slot);

        return slot;
    }

    /// <summary>
    /// <br>Удаляет слот</br>
    /// <br>[Не рекомендуеться]</br>
    /// <br>Используйте AddToItemCount, SetItemCount</br>
    /// </summary>
    public void DeleteSlot(InventorySlot slot) => slots.Remove(slot);

    public void DeleteSlotByItemName(string name)
    {
        InventorySlot slot = GetSlotByItemName(name);

        if (slot != null)
            DeleteSlot(slot);
    }
    public void DeleteSlotByItem(RPGCollectable item)
    {
        InventorySlot slot = this[item];

        if (slot != null)
            DeleteSlot(slot);
    }

    /// <summary>
    /// Проверяет наличие слота под этот предмет
    /// </summary>
    public bool HasItemSlot(RPGCollectable item)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.Item == item)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Добавляет значение к количеству предметов
    /// </summary>
    /// <param name="item">Предмет</param>
    /// <param name="value">Значение</param>
    public void AddToItemCount(RPGCollectable item, int value)
    {
        InventorySlot slot = this[item.Tag];

        if (slot == null && value <= 0)
        {
            Debug.LogWarning("Предмет не найден в инвентаре");
            return;
        }           

        slot ??= CreateSlot(item);

        slot.Count += value;

        if (slot.Count <= 0)
            DeleteSlotByItemName(item.Tag);
    }
    /// <summary>
    /// Устанавлиет кол-во предметов
    /// </summary>
    /// <param name="item">Предмет</param>
    /// <param name="value">Значение</param>
    public void SetItemCount(RPGCollectable item, int value)
    {
        InventorySlot slot = GetSlotByItemName(item.name);

        if (slot == null && value <= 0)
        {
            Debug.LogWarning("Предмет не найден в инвенторе");
            return;
        }

        slot ??= CreateSlot(item);

        slot.Count = value;

        if (slot.Count <= 0)
            DeleteSlotByItemName(item.name);
    }

    public IEnumerator GetEnumerator()
    {
        return Slots.GetEnumerator();
    }

    IEnumerator<InventorySlot> IEnumerable<InventorySlot>.GetEnumerator()
    {
        return (IEnumerator<InventorySlot>)Slots.GetEnumerator();
    }

    public void Dispose()
    {
       slots.Clear();
    }
}
