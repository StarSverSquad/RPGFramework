using RPGF.Core.Inventory;
using RPGF.GUI.Abstractions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GlackSaga.GUI.TittleMenu.Items
{
    public class ItemsListItem : GUIInteractableBase
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
            text.text = GetLocale($"ITEM_{slot.Item.Tag}", slot.Item.Name);
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
