using RPGF.UI;
using RPGF.RPG;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RPGF.GUI.Abstractions;

namespace GlackSaga.GUI.TitleMenu
{
    class CharacterInformationGUI : GUIElementBase
    {
        [SerializeField]
        private LineBar _hpBar;
        [SerializeField]
        private LineBar _mpBar;

        [SerializeField]
        private TextMeshProUGUI _hpText;
        [SerializeField]
        private TextMeshProUGUI _mpText;

        [SerializeField]
        private TextMeshProUGUI _name;

        [SerializeField]
        private Image _icon;

        public void Initialize(RPGCharacter character)
        {
            _hpBar.SetValue((float)character.Heal / (float)character.MaxHeal);
            _mpBar.SetValue((float)character.Mana / (float)character.MaxMana);

            _hpText.text = $"{character.Heal} / {character.MaxHeal}";
            _mpText.text = $"{character.Mana} / {character.MaxMana}";

            _name.text = GetLocale(character.Name);

            _icon.sprite = character.Icon;
        }
    }
}