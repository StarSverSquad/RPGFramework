using GlackSaga.GUI.TitleMenu;
using GlackSaga.GUI.TittleMenu.CharactetSelector;
using NaughtyAttributes;
using RPGF.Core.Character;
using RPGF.Core.Inventory;
using RPGF.Core.RPGEffect;
using RPGF.Core.Services;
using RPGF.Domain.DI;
using RPGF.GUI;
using RPGF.RPG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GlackSaga.GUI.TittleMenu.Items
{
    public class ItemsGUIBlock : GUISelectableBlock
    {
        public enum ItemsTab
        {
            Regular = 0,
            Key = 1
        }

        [Inject]
        private readonly InventoryService _inventoryService;
        [Inject]
        private readonly CharacterService _characterService;
        [Inject]
        private readonly InvokeUsableEventService _invokeUsableEventService;

        #region Links

        [Header("Items block options:")] 
        [SerializeField]
        private ItemsGUITab[] itemsGUITabs = new ItemsGUITab[2];
        [SerializeField]
        private CharacterSelectorGUIBlock characterSelector;
        [SerializeField]
        private CharacterInformationManager characterInformation;
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
        [Space]
        [SerializeField]
        private AudioSource DenySound;
        [SerializeField]
        private AudioSource UseSound;

        #endregion

        public int Page { get; private set; } = 0;
        public int PageSize => Elements.Count;
        public int MaxPage => Mathf.CeilToInt((float)selectedSlots.Count / PageSize) - 1;

        public int AbsoluteIndex => CurrentIndex + (Page * PageSize);

        public ItemsTab SelectedTab { get; private set; } = ItemsTab.Regular;

        private List<InventorySlotData> selectedSlots = new();

        private Coroutine usingItemCoroutine = null;

        #region Events

        [Foldout("Items block events")]
        public UnityEvent OnTabChanged;

        #endregion

        protected override void ChangeSelect(int newIndex)
        {
            var absoluteIndex = newIndex + (Page * PageSize);
            if (absoluteIndex >= selectedSlots.Count || absoluteIndex < 0)
            {
                return;
            }

            if (Page > 0 && newIndex < 0)
            {
                SetPage(Page - 1);
                base.ChangeSelect(PageSize - 1);
            }
            else if (Page < MaxPage && newIndex >= PageSize)
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

            if (page < MaxPage)
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

        protected override void OnChoiced(int index)
        {
            var absoluteIndex = index + (Page * PageSize);
            var slot = selectedSlots[absoluteIndex];

            if (usingItemCoroutine is not null)
                StopCoroutine(usingItemCoroutine);

            usingItemCoroutine = StartCoroutine(UseItemPipeline(slot));
        }

        protected override void OnSelectUpdate()
        {
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

        private IEnumerator UseItemPipeline(InventorySlotData slot)
        {
            var item = slot.Item;

            if (item.Usage == Usability.Battle)
            {
                DenySound.Play();
                StartChoice();
                yield break;
            }

            RPGCharacter[] targets;
            switch (item.Direction)
            {
                case UsabilityDirection.Any:
                case UsabilityDirection.Teammate:

                    Next(characterSelector);

                    yield return new WaitWhile(() => characterSelector.IsActivated);

                    if (characterSelector.Canceled)
                    {
                        StartChoice();
                        yield break;
                    }

                    targets = new[] { characterSelector.SeletedCharacter };

                    break;
                case UsabilityDirection.All:
                case UsabilityDirection.AllTeam:
                    targets = _characterService.Characters;
                    break;
                default:
                    DenySound.Play();
                    StartChoice();
                    yield break;
            }

            foreach (var target in targets)
            {
                foreach (var effect in item.Effects)
                {
                    yield return effect.Invoke(target, target);
                }
            }

            if (_inventoryService[item].Count == 1)
            {
                if (CurrentIndex == 0 && Page > 0)
                {
                    SetPage(Page - 1);
                    ChangeSelect(PageSize - 1);
                }
                else
                {
                    ChangeSelect(CurrentIndex - 1);
                }
            }

            _inventoryService.AddToItemCount(item, -1);
            UpdateItems();

            characterInformation.UpdateInformation();
            UseSound.Play();

            if (item.Event != null)
            {
                _invokeUsableEventService.InvokeEvent(item);

                Manager.Close();
            }
            else
            {
                StartChoice();
            }
        }
    }
}
