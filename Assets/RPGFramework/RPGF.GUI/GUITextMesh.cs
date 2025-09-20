using TMPro;
using UnityEngine;

namespace RPGF.GUI
{
    // TODO: Убрать
    public class GUITextMesh : TextMeshProUGUI
    {
        [SerializeField]
        private bool _autoLocalize = true;

        protected override void OnEnable()
        {
            if (_autoLocalize)
                text = GameManager.Instance.Localization.GetLocale(text);
        }
    }
}
