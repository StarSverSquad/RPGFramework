using System;
using RPGF.Core.Location;
using RPGF.Core.SaveLoad;
using RPGF.Domain.Interfaces;
using RPGF.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GlackSaga.GUI.TitleMenu.SaveLoad
{
    public class SaveSlotItem : GUIInteractable, ISetData<GameSlotData>
    {
        [SerializeField]
        private TextMeshProUGUI text;
        [SerializeField]
        private TextMeshProUGUI locationText;
        [SerializeField]
        private TextMeshProUGUI timeText;
        [SerializeField]
        private Image background;

        public void SetData(GameSlotData data)
        {
            text.text = $"{GetLocale("SYS_SAVE_SLOT")} {data.Id}";
            locationText.text = GetLocale(RpgfLocationInfo.GetLocaleNameTag(data.LocationTag));

            timeText.text = DateTime.Parse(data.SaveDateTime).ToString("dd.MM.yyyy, HH:mm:ss");
        }

        public void SetNewSlot()
        {
            text.text = GetLocale("SYS_SAVE_NEW");
            locationText.text = string.Empty;
            timeText.text = string.Empty;
        }

        public override void OnFocused()
        {
            background.enabled = true;
        }

        public override void OnUnfocused()
        {
            background.enabled = false;
        }
    }
}