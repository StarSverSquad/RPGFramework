using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlackSaga.GUI.TitleMenu.CharactetSelector;
using NaughtyAttributes;
using RPGF.Core.Character;
using RPGF.Core.Inventory;
using RPGF.Core.RPGEffect;
using RPGF.Core.Services;
using RPGF.Domain.DI;
using RPGF.RPG;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GlackSaga.GUI.TitleMenu.Items
{
    public class ItemsGUIBlock : InventoryPaginatedGUIBlock
    {
        public enum ItemsTab
        {
            Regular = 0,
            Key = 1
        }

        [Inject]
        private readonly CharacterService _characterService = null!;
        [Inject]
        private readonly InvokeUsableEventService _invokeUsableEventService = null!;

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

        public ItemsTab SelectedTab { get; private set; } = ItemsTab.Regular;

        private Coroutine usingItemCoroutine = null;

        #region Events

        [Foldout("Items block events")]
        public UnityEvent OnTabChanged;

        #endregion

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

            RefreshItems();

            if (HasItems)
            {
                ChangeSelect(0);
            }
        }

        protected override IEnumerable<InventorySlotData> FilterSlots(IEnumerable<InventorySlotData> slots)
        {
            return slots.Where(i => SelectedTab == ItemsTab.Key
                ? i.Item.Rare == Rareness.Key
                : i.Item.Rare != Rareness.Key);
        }

        protected override void OnItemsRefreshed()
        {
            if (!HasItems)
            {
                emptyText.gameObject.SetActive(true);
                UpdateItemDetails(null);
            }
            else
            {
                emptyText.gameObject.SetActive(false);
            }
        }

        protected override void BindElement(int elementIndex, InventorySlotData slot)
        {
            BindElementWithInterface(elementIndex, slot);
        }

        protected override void HideElement(int elementIndex)
        {
            HideElementAt(elementIndex);
        }

        protected override void UpdatePaginationArrows(int page, int maxPage)
        {
            arrowUp.gameObject.SetActive(page > 0);
            arrowDown.gameObject.SetActive(page < maxPage);
        }

        protected override void OnItemSelected(InventorySlotData slot)
        {
            UpdateItemDetails(slot);
        }

        protected override void OnChoiced(int index)
        {
            var slot = GetItemAt(ToAbsoluteIndex(index));

            if (usingItemCoroutine is not null)
            {
                StopCoroutine(usingItemCoroutine);
            }

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
            {
                item.SetFocus(false);
            }

            SelectedTab = ItemsTab.Regular;
            UpdateItemDetails(null);
        }

        private void UpdateItemDetails(InventorySlotData slot)
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

            Inventory.AddToItemCount(item, -1);
            RefreshItems();

            if (HasItems)
            {
                ChangeSelect(Mathf.Min(CurrentIndex, PageSize - 1));
            }
            else
            {
                UpdateItemDetails(null);
            }

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
