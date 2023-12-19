using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
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
    /// ���������� ������ ���� ������������ ���������
    /// </summary>
    public List<RPGConsumed> GetConsumeds()
    {
        return slots.Where(i => i.Item is RPGConsumed).Select(i => i.Item as RPGConsumed).ToList();
    }
    /// <summary>
    /// ���������� ������ ���� ���������� ���������
    /// </summary>
    public List<RPGWerable> GetWerables()
    {
        return slots.Where(i => i.Item is RPGWerable).Select(i => i.Item as RPGWerable).ToList();
    }

    public InventorySlot GetSlotByItemName(string name)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.Item.Name == name)
                return slot;
        }

        return null;
    }

    /// <summary>
    /// <br>������ ������ ����</br>
    /// <br>[�� ��������������]</br>
    /// <br>����������� AddToItemCount, SetItemCount</br>
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
    /// <br>������� ����</br>
    /// <br>[�� ��������������]</br>
    /// <br>����������� AddToItemCount, SetItemCount</br>
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
    /// ��������� ������� ����� ��� ���� �������
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
    /// ��������� �������� � ���������� ���������
    /// </summary>
    /// <param name="item">�������</param>
    /// <param name="value">��������</param>
    public void AddToItemCount(RPGCollectable item, int value)
    {
        InventorySlot slot = this[item.Name];

        if (slot == null && value <= 0)
        {
            Debug.LogWarning("������� �� ������ � ���������");
            return;
        }           

        slot ??= CreateSlot(item);

        slot.Count += value;

        if (slot.Count <= 0)
            DeleteSlotByItemName(item.Name);
    }
    /// <summary>
    /// ����������� ���-�� ���������
    /// </summary>
    /// <param name="item">�������</param>
    /// <param name="value">��������</param>
    public void SetItemCount(RPGCollectable item, int value)
    {
        InventorySlot slot = GetSlotByItemName(item.name);

        if (slot == null && value <= 0)
        {
            Debug.LogWarning("������� �� ������ � ���������");
            return;
        }

        slot ??= CreateSlot(item);

        slot.Count = value;

        if (slot.Count <= 0)
            DeleteSlotByItemName(item.name);
    }
}