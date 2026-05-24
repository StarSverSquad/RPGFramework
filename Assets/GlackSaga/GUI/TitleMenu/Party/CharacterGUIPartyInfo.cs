using RPGF.Core;
using RPGF.UI;
using TMPro;
using UnityEngine;

namespace GlackSaga.GUI.TitleMenu.Party
{
    public class CharacterGUIPartyInfo : RPGFrameworkBehaviour
    {
        [SerializeField]
        private PartyGUIBlock _partyBlock;
        [Space]
        [SerializeField]
        private LineBar _hpBar;
        [SerializeField]
        private LineBar _mpBar;
        [Space]
        [SerializeField]
        private TextMeshProUGUI _hpTxt;
        [SerializeField]
        private TextMeshProUGUI _mpTxt;
        [Space]
        [SerializeField]
        private TextMeshProUGUI _dmgTxt;
        [SerializeField]
        private TextMeshProUGUI _defTxt;
        [SerializeField]
        private TextMeshProUGUI _agiTxt;
        [SerializeField]
        private TextMeshProUGUI _luckTxt;
        [Space]
        [SerializeField]
        private CharacterAbilityItem[] _guiAbilitiesItems = new CharacterAbilityItem[4];

        public void UpdateData()
        {
            var character = Global.Character.Characters[_partyBlock.CurrentIndex];

            _hpBar.SetValue(character.Heal, character.MaxHeal);
            _mpBar.SetValue(character.Mana, character.MaxMana);

            _hpTxt.text = $"{character.Heal} / {character.MaxHeal}";
            _mpTxt.text = $"{character.Mana} / {character.MaxMana}";

            _dmgTxt.text = $"{Global.Localization.GetLocale("SYS_DMG")}: {character.Damage}";
            _defTxt.text = $"{Global.Localization.GetLocale("SYS_DEF")}: {character.Defense}";
            _agiTxt.text = $"{Global.Localization.GetLocale("SYS_AGI")}: {character.Agility}";
            _luckTxt.text = $"{Global.Localization.GetLocale("SYS_LUCK")}: {character.Luck}";

            for (int i = 0; i < _guiAbilitiesItems.Length; i++)
            {
                var item = _guiAbilitiesItems[i];

                if (i < character.Abilities.Count)
                {
                    item.SetData(character.Abilities[i]);
                    item.gameObject.SetActive(true);
                }
                else
                {
                    item.gameObject.SetActive(false);
                }
            }
        }
    }

}