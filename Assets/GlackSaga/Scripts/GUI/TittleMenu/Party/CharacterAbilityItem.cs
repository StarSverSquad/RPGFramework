using RPGF.RPG;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GlackSaga.GUI.TitleMenu.Party
{
    public class CharacterAbilityItem : RPGFrameworkBehaviour
    {
        [SerializeField]
        private Image _icon;
        [SerializeField]
        private TextMeshProUGUI _name;

        public void SetData(RPGAbility ability)
        {
            _icon.sprite = ability.Icon;
            _name.text = GetLocale(ability.Name);
        }
    }
}
