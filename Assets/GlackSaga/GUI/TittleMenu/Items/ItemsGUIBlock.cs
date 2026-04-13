using NaughtyAttributes;
using RPGF.Core.Inventory;
using RPGF.Domain.DI;
using RPGF.GUI;
using RPGF.RPG;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GlackSaga.GUI.TittleMenu.Items
{
    public class ItemsGUIBlock : GUIChoiceBlock
    {
        public enum ItemsTab
        {
            Regular = 0,
            Key = 1
        }

        [Inject]
        private readonly InventoryService _inventoryService;

        #region Links

        [Header("Items block options:")] 
        [SerializeField]
        private ItemsGUITab[] itemsGUITabs = new ItemsGUITab[2];
        [Header("Links:")]
        [SerializeField]
        private Image arrowUp;
        [SerializeField] 
        private Image arrowDown;
        [Space]
        [SerializeField]
        private TextMeshProUGUI emptyText;
        [Space]
        [SerializeField]
        private TextMeshProUGUI itemName;
        [SerializeField]
        private TextMeshProUGUI hpGain;
        [SerializeField]
        private TextMeshProUGUI mpGain;
        [SerializeField]
        private TextMeshProUGUI description;

        #endregion

        public int Page { get; private set; } = 0;
        public int PageSize => Elements.Count;
        public int MaxPage => Mathf.CeilToInt(selectedSlots.Count / PageSize);

        public int AbsoluteIndex => CurrentElementIndex + (Page * PageSize);

        public ItemsTab SelectedTab { get; private set; } = ItemsTab.Regular;

        private List<InventorySlotData> selectedSlots = new();

        #region Events

        [Foldout("Items block events")]
        public UnityEvent OnTabChanged;

        #endregion

        private void Update()
        {
            if (!IsActivated)
                return;

            ItemsTab tab = SelectedTab;
            if (Input.GetKeyDown(Global.BaseOptions.MoveRight))
            {
                tab = ItemsTab.Key;
            }
            else if (Input.GetKeyDown(Global.BaseOptions.MoveLeft))
            {
                tab = ItemsTab.Regular;
            }

            if (tab != SelectedTab)
            {
                SelectedTab = tab;
                UpdateTab();
                OnTabChanged?.Invoke();
            }
        }

        protected override void ChangeSelect(int newIndex)
        {
            var absoluteIndex = newIndex + (Page * PageSize);
            if (absoluteIndex >= selectedSlots.Count || absoluteIndex < 0)
            {
                return;
            }

            if (Page > 0 && newIndex >= PageSize)
            {
                SetPage(Page - 1);
                base.ChangeSelect(PageSize - 1);
            }
            else if (Page < MaxPage && newIndex < 0)
            {
                SetPage(Page + 1);
                base.ChangeSelect(0);
            }
            else
            {
                base.ChangeSelect(newIndex);
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            UpdateTab();
        }

        private void UpdateTab()
        {
            for (int tabIndex = 0; tabIndex < Enum.GetNames(typeof(ItemsTab)).Length; tabIndex++)
            {
                var tab = itemsGUITabs[tabIndex];
                tab.SetFocus(tabIndex == (int)SelectedTab);
            }

            UpdateItems();

            ChangeSelect(0);
        }

        private void UpdateItems()
        {
            selectedSlots = _inventoryService.Slots
                .Where(i => SelectedTab == ItemsTab.Key ? i.Item.Rare == Rareness.Key : i.Item.Rare != Rareness.Key)
                .ToList();

            SetPage(0);
        }

        private void SetPage(int page)
        {
            this.Page = page;

            for (int i = 0; i < PageSize; i++)
            {
                var itemGUI = Elements[i] as ItemsListItem;
                var slotIndex = i + (page * PageSize);
                if (slotIndex >= selectedSlots.Count)
                {
                    itemGUI.gameObject.SetActive(false);

                    continue;
                }

                itemGUI.SetData(selectedSlots[slotIndex]);
                itemGUI.gameObject.SetActive(true);
            }

            if (page == 0)
            {
                arrowUp.gameObject.SetActive(false);
            }
            else
            {
                arrowUp.gameObject.SetActive(true);
            }

            if (page < MaxPage - 1)
            {
                arrowDown.gameObject.SetActive(true);
            }
            else
            {
                arrowDown.gameObject.SetActive(false);
            }
        }

        private void SetData(InventorySlotData slot)
        {
            if (slot is not null)
            {

            }
            else
            {

            }
        }

        protected override void OnDiativate()
        {
            base.OnDiativate();

            foreach (var item in itemsGUITabs)
                item.SetFocus(false);

            SelectedTab = ItemsTab.Regular;

            Page = 0;

            selectedSlots.Clear();
        }
    }
}
