using RPGF.Core;
using RPGF.Domain.Interfaces;
using RPGF.GUI;
using RPGF.RPG;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GlackSaga.GUI.TitleMenu.FullInfo
{
    public class FullInfoGearItem : GUIWidget, ISetData<RPGWearable>
    {
        [SerializeField]
        private Image _icon;
        [SerializeField]
        private TextMeshProUGUI _name;

        public void SetData(RPGWearable wearable)
        {
            if (wearable == null)
            {
                _icon.enabled = false;

                _name.text = GetLocale("SYS_EMPTY");
            }
            else
            {
                _icon.enabled = true;

                _icon.sprite = wearable.Icon;

                _name.text = GetLocale(wearable.Name);
            }
        }
    }
}
