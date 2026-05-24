using RPGF.Core.Inventory;
using RPGF.Domain.Interfaces;
using RPGF.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GlackSaga.GUI.TitleMenu.Items
{
    public class ItemsListItem : GUIInteractable, ISetData<InventorySlotData>
    {
        [SerializeField]
        private Image icon;
        [SerializeField]
        private TextMeshProUGUI text;
        [SerializeField]
        private TextMeshProUGUI counter;
        [Space]
        [SerializeField]
        private Color focusColor = Color.white;

        public void SetData(InventorySlotData slot)
        {
            icon.sprite = slot.Item.Icon;
            text.text = GetLocale(slot.Item.GetLocaleNameTag(), slot.Item.Name);
            counter.text = $"{slot.Count}x";
        }

        public override void OnFocused()
        {
            text.color = focusColor;
        }

        public override void OnUnfocused()
        {
            text.color = Color.white;
        }
    }
}
