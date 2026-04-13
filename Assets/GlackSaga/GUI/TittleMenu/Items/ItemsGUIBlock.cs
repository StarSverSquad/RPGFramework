using NaughtyAttributes;
using RPGF.Core.Inventory;
using RPGF.Core.RPGEffect;
using RPGF.Domain.DI;
using RPGF.GUI;
using RPGF.RPG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            SetData(selectedSlots[absoluteIndex]);
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

            if (selectedSlots.Count == 0)
            {
                emptyText.gameObject.SetActive(true);
                SetData(null);
            }
            else
            {
                emptyText.gameObject.SetActive(false);
            }

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
                itemName.text = GetLocale(slot.Item.GetLocaleNameTag(), slot.Item.Name);
                description.text = GetLocale(slot.Item.GetLocaleDesciptionTag(), slot.Item.Description);

                var changeManaHealByConst = slot.Item.Effects.OfType<ChangeManaHealConstEffect>();
                var changeManaHealByPersent = slot.Item.Effects.OfType<ChangeManaHealPercentEffect>();

                int hpCountGain = changeManaHealByConst.Sum(i => i.Heal);
                float hpPercentGain = changeManaHealByPersent.Sum(i => i.Heal);

                int mpCountGain = changeManaHealByConst.Sum(i => i.Mana);
                float mpPercentGain = changeManaHealByPersent.Sum(i => i.Mana);

                StringBuilder hpGainTextBuilder = new();
                if (hpCountGain > 0 || hpPercentGain > 0)
                {
                    hpGainTextBuilder.Append("+ ");
                }
                if (hpCountGain > 0)
                {
                    hpGainTextBuilder.Append(hpCountGain);
                }
                if (hpCountGain > 0 && hpPercentGain > 0)
                {
                    hpGainTextBuilder.Append(", + ");
                }
                if (hpPercentGain > 0)
                {
                    hpGainTextBuilder.Append($"{Mathf.RoundToInt(hpPercentGain * 100)}%");
                }
                if (hpCountGain > 0 || hpPercentGain > 0)
                {
                    hpGainTextBuilder.Append(" [HP]");
                }


                StringBuilder mpGainTextBuilder = new();
                if (mpCountGain > 0 || mpPercentGain > 0)
                {
                    mpGainTextBuilder.Append("+ ");
                }
                if (mpCountGain > 0)
                {
                    mpGainTextBuilder.Append(mpCountGain);
                }
                if (mpCountGain > 0 && mpPercentGain > 0)
                {
                    mpGainTextBuilder.Append(", + ");
                }
                if (mpPercentGain > 0)
                {
                    mpGainTextBuilder.Append($"{Mathf.RoundToInt(mpPercentGain * 100)}%");
                }
                if (mpCountGain > 0 || mpPercentGain > 0)
                {
                    mpGainTextBuilder.Append(" [MP]");
                }

                hpGain.text = hpGainTextBuilder.ToString();
                mpGain.text = mpGainTextBuilder.ToString();
            }
            else
            {
                itemName.text = string.Empty;
                description.text = string.Empty;
                hpGain.text = string.Empty;
                mpGain.text = string.Empty;
            }
        }

        protected override void OnDiativate()
        {
            base.OnDiativate();

            foreach (var item in itemsGUITabs)
                item.SetFocus(false);

            SelectedTab = ItemsTab.Regular;

            Page = 0;

            SetData(null);

            selectedSlots.Clear();
        }
    }
}
