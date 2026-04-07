using RPGF.Core;
using RPGF.Domain.DI;
using RPGF.GUI;
using RPGF.GUI.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GlackSaga.GUI.TitleMenu.Items
{
    /// <summary>
    /// 
    /// </summary>
    public class ItemsGUIBlockUIToolkitTest : GUIBlockBase
    {
        public enum ItemsTabs
        {
            Regular = 0, 
            Important = 1
        }

        private const string TabSelectedClass = "tab-selected";
        private const string ListItemSelectedClass = "content-list-item-selected";

        [SerializeField]
        private UIDocument document;

        public ItemsTabs SelectedTab { get; private set; } = ItemsTabs.Regular;

        private List<VisualElement> tabs;
        private List<VisualElement> listItems;

        private Image selectedItemImage;
        private Label selectedItemName;
        private Label selectedItemHpGain;
        private Label selectedItemMpGain;
        private Label selectedItemDescription;

        public override void Initialize(IGUIManager manager)
        {
            base.Initialize(manager);

            tabs = document.rootVisualElement.Query(className: "tab").ToList();
            listItems = document.rootVisualElement.Query(className: "content-list-item").ToList();

            selectedItemImage = document.rootVisualElement.Q<Image>("ItemImage");
            selectedItemName = document.rootVisualElement.Q<Label>("ItemName");
            selectedItemHpGain = document.rootVisualElement.Q<Label>("ItemHpGain");
            selectedItemMpGain = document.rootVisualElement.Q<Label>("ItemMpGain");
            selectedItemDescription = document.rootVisualElement.Q<Label>("ItemDescription");
        }

        private void Update()
        {
            ItemsTabs tab = SelectedTab;
            if (Input.GetKeyDown(Global.BaseOptions.MoveRight))
            {
                tab = ItemsTabs.Important;
            }
            else if (Input.GetKeyDown(Global.BaseOptions.MoveLeft))
            {
                tab = ItemsTabs.Regular;
            }

            if (tab != SelectedTab)
            {
                UpdateTab();
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            UpdateTab();
        }

        private void UpdateTab()
        {
            for (int tabIndex = 0; tabIndex < Enum.GetNames(typeof(ItemsTabs)).Length; tabIndex++)
            {
                var tab = tabs[tabIndex];
                if (tabIndex == (int)SelectedTab)
                {
                    tab.AddToClassList(TabSelectedClass);
                }
                else
                {
                    tab.RemoveFromClassList(TabSelectedClass);
                }
            }
        }

        protected override void OnDiativate()
        {
            base.OnDiativate();

            SelectedTab = ItemsTabs.Regular;
        }
    }
}
