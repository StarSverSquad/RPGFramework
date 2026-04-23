using RPGF.Domain.Interfaces;
using RPGF.GUI;
using RPGF.RPG;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GlackSaga.GUI.TitleMenu.FullInfo
{
    public class FullInfoBlockGUI : GUIBlock, ISetData<RPGCharacter>
    {
        [SerializeField]
        private Image _charImage;
        [SerializeField]
        private Image _charGradient;
        [Space]
        [SerializeField]
        private TextMeshProUGUI _charName;
        [SerializeField]
        private TextMeshProUGUI _charDescription;
        [SerializeField]
        private TextMeshProUGUI _charClass;
        [SerializeField]
        private TextMeshProUGUI _charLevel;
        [Space]
        [SerializeField]
        private TextMeshProUGUI _charDmg;
        [SerializeField]
        private TextMeshProUGUI _charDef;
        [SerializeField]
        private TextMeshProUGUI _charAgi;
        [SerializeField]
        private TextMeshProUGUI _charLuck;
        [Space]
        [SerializeField]
        private FullInfoGearItem _weaponItem;
        [SerializeField]
        private FullInfoGearItem _headItem;
        [SerializeField]
        private FullInfoGearItem _bodyItem;
        [SerializeField]
        private FullInfoGearItem _shieldItem;
        [SerializeField]
        private FullInfoGearItem _talismanItem;
        [Space]
        [SerializeField]
        private List<FullInfoAbilityItem> _abilitiesItems = new();

        private void Update()
        {
            if (IsActivated && Input.GetKeyDown(Global.BaseOptions.Cancel))
            {
                Preview();
            }
        }

        public void SetData(RPGCharacter value)
        {
            _charImage.sprite = value.Icon;
            _charGradient.color = value.Color;

            _charName.text = GetLocale(value.Name);
            _charDescription.text = GetLocale(value.Description);

            _charClass.text = $"{GetLocale("SYS_CLASS")}: {GetLocale(value.Class)}";
            _charLevel.text = $"{GetLocale("SYS_LEVEL")}: {value.Level}";

            _charDmg.text = $"{GetLocale("SYS_DMG")}: {value.Damage}";
            _charDef.text = $"{GetLocale("SYS_DEF")}: {value.Defence}";
            _charAgi.text = $"{GetLocale("SYS_AGI")}: {value.Agility}";
            _charLuck.text = $"{GetLocale("SYS_LUCK")}: {value.Luck}";

            _weaponItem.SetData(value.WeaponSlot);
            _headItem.SetData(value.HeadSlot);
            _bodyItem.SetData(value.BodySlot);
            _shieldItem.SetData(value.ShieldSlot);
            _talismanItem.SetData(value.TalismanSlot);

            foreach (var item in _abilitiesItems)
                item.gameObject.SetActive(false);

            for (int i = 0; i < Mathf.Min(3, value.Abilities.Count); i++)
            {
                var ability = value.Abilities[i];
                var item = _abilitiesItems[i];

                item.SetData(ability);
                item.gameObject.SetActive(true);
            }
        }
    }
}
