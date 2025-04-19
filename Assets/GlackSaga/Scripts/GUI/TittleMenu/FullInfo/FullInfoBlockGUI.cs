using RPGF.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GlackSaga.GUI.TitleMenu.FullInfo
{
    public class FullInfoBlockGUI : GUIBlockBase
    {
        [SerializeField]
        private Image _charImage;
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
    }
}
