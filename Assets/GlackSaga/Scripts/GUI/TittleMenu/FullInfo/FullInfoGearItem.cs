using RPGF.Core;
using RPGF.Domain.Interfaces;
using RPGF.RPG;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GlackSaga.GUI.TitleMenu.FullInfo
{
    public class FullInfoGearItem : RPGFrameworkBehaviour, ISetData<RPGWerable>
    {
        [SerializeField]
        private Image _icon;
        [SerializeField]
        private TextMeshProUGUI _name;

        public void SetData(RPGWerable werable)
        {
            if (werable == null)
            {
                _icon.enabled = false;

                _name.text = GetLocale("SYS_EMPTY");
            }
            else
            {
                _icon.enabled = true;

                _icon.sprite = werable.Icon;

                _name.text = GetLocale(werable.Name);
            }
        }
    }
}
