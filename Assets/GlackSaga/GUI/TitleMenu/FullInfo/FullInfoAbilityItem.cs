using RPGF.Core;
using RPGF.Domain.Interfaces;
using RPGF.GUI;
using RPGF.RPG;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GlackSaga.GUI.TitleMenu.FullInfo
{
    public class FullInfoAbilityItem : GUIWidget, ISetData<RPGAbility>
    {
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TextMeshProUGUI _name;
        [SerializeField]
        private TextMeshProUGUI _description;
        [SerializeField]
        private TextMeshProUGUI _mana;
        [SerializeField]
        private TextMeshProUGUI _concentration;

        public void SetData(RPGAbility value)
        {
            _icon.sprite = value.Icon;

            _name.text = GetLocale(value.Name);
            _description.text = GetLocale(value.Description);

            _mana.text = $"{GetLocale("SYS_MANA")}: {value.ManaCost}";
            _concentration.text = $"{GetLocale("SYS_CONCENTRATION")}: {value.ConcentrationCost}";
        }
    }
}
