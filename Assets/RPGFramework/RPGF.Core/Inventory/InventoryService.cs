using RPGF.RPG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPGF.Core.Inventory
{
    public class InventoryService : IEnumerable<InventorySlotData>, IDisposable
    {
        private InventoryData _data;

        public InventorySlotData[] Slots => _data.Slots.ToArray();

        public InventoryService()
        {
            _data = new InventoryData();
        }

        public InventorySlotData this[string itemTag]
        {
            get => GetSlotByItemTag(itemTag);
        }
        public InventorySlotData this[RPGCollectable item]
        {
            get => _data.Slots.First(i => i.Item == item);
        }


        public List<RPGConsumed> GetConsumeds()
        {
            return _data.Slots.Where(i => i.Item is RPGConsumed).Select(i => i.Item as RPGConsumed).ToList();
        }
        public List<RPGWerable> GetWerables()
        {
            return _data.Slots.Where(i => i.Item is RPGWerable).Select(i => i.Item as RPGWerable).ToList();
        }

        public InventorySlotData GetSlotByItemTag(string name)
        {
            foreach (InventorySlotData slot in _data.Slots)
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
        public InventorySlotData CreateSlot(RPGCollectable slotfor)
        {
            InventorySlotData slot = new InventorySlotData()
            {
                Item = slotfor,
                Count = 0
            };

            _data.Slots.Add(slot);

            return slot;
        }

        /// <summary>
        /// <br>Удаляет слот</br>
        /// <br>[Не рекомендуеться]</br>
        /// <br>Используйте AddToItemCount, SetItemCount</br>
        /// </summary>
        public void DeleteSlot(InventorySlotData slot) => _data.Slots.Remove(slot);

        public void DeleteSlotByItemName(string name)
        {
            InventorySlotData slot = GetSlotByItemTag(name);

            if (slot != null)
                DeleteSlot(slot);
        }
        public void DeleteSlotByItem(RPGCollectable item)
        {
            InventorySlotData slot = this[item];

            if (slot != null)
                DeleteSlot(slot);
        }

        /// <summary>
        /// Проверяет наличие слота под этот предмет
        /// </summary>
        public bool HasItemSlot(RPGCollectable item)
        {
            foreach (InventorySlotData slot in _data.Slots)
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
            InventorySlotData slot = this[item.Tag];

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
            InventorySlotData slot = GetSlotByItemTag(item.name);

            if (slot == null && value <= 0)
            {
                Debug.LogWarning("Предмет не найден в инвентаре");
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

        IEnumerator<InventorySlotData> IEnumerable<InventorySlotData>.GetEnumerator()
        {
            return (IEnumerator<InventorySlotData>)Slots.GetEnumerator();
        }

        public void Dispose()
        {
            _data.Slots.Clear();
        }
    }
}