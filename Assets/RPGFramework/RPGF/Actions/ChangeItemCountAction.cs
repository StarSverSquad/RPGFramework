using RPGF.Core.Inventory;
using RPGF.Domain.DI;
using RPGF.EventSystem;
using RPGF.EventSystem.Attributes;
using RPGF.RPG;
using System;
using System.Collections;

namespace RPGF.Actions
{
    [GenerateActionNode("Измененить количество предмета", contextMenuPath: "Система/Измененить количество предмета")]
    [Serializable]
    public class ChangeItemCountAction : ActionBase
    {
        [Inject]
        private readonly InventoryService _inventory;

        [ActionFieldOption("Предмет", AllowSceneObjects = true)]
        public RPGCollectable Item;
        [ActionFieldOption("Количество")]
        public int Count;
        [ActionFieldOption("Установить/Добавить?")]
        public bool IsSet;

        public ChangeItemCountAction() : base()
        {
            Item = null;
            Count = 0;
            IsSet = false;
        }

        public override IEnumerator ActionCoroutine()
        {
            if (IsSet)
            {
                _inventory.SetItemCount(Item, Count);
            }
            else
            {
                _inventory.AddToItemCount(Item, Count);
            }

            yield break;
        }
    }
}