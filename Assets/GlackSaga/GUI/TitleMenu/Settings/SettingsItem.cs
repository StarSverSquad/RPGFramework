using RPGF.GUI;
using TMPro;
using UnityEngine;

namespace GlackSaga.GUI.TitleMenu.Settings
{
    public class SettingsItem : GUIInteractable
    {
        [SerializeField]
        private TextMeshProUGUI labelGui;

        [SerializeField]
        private Color focusColor = Color.white;
        [SerializeField]
        private Color unfocusColor = Color.white;

        public override void OnFocused()
        {
            labelGui.color = focusColor;
        }

        public override void OnUnfocused()
        {
            labelGui.color = unfocusColor;
        }
    }
}
