using RPGF.Core.Inventory;
using RPGF.Domain.Interfaces;
using RPGF.GUI;
using RPGF.RPG;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GlackSaga.GUI.TitleMenu.Gear
{
    public class GearSelectionItem : GUIInteractable, ISetData<InventorySlotData>
    {
        [Header("LINKS:")]
        [SerializeField]
        private Image icon;
        [SerializeField]
        private TextMeshProUGUI nameText;
        [SerializeField]
        private TextMeshProUGUI countText;

        [SerializeField]
        private Color focusedColor = Color.orange;
        [SerializeField]
        private Color unfocusedColor = Color.white;

        [SerializeField]
        private Sprite unequipSprite;

        public void SetData(InventorySlotData value)
        {
            if (value == null)
            {
                SetUnequip();
                return;
            }

            icon.enabled = true;
            icon.sprite = value.Item.Icon;
            nameText.text = GetLocale(value.Item.GetLocaleNameTag(), value.Item.Name);

            if (value.Count > 1)
            {
                countText.text = $"{value.Count}x";
            }
            else
            {
                countText.text = string.Empty;
            }
        }

        public void SetUnequip()
        {
            icon.sprite = unequipSprite;
            nameText.text = GetLocale("SYS_NOTHING");
            countText.text = string.Empty;
        }

        public override void OnFocused()
        {
            base.OnFocused();

            nameText.color = focusedColor;
        }

        public override void OnUnfocused()
        {
            base.OnUnfocused();

            nameText.color = unfocusedColor;
        }
    }
}
