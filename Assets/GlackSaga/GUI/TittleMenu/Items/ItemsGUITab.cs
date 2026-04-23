using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GlackSaga.GUI.TittleMenu.Items
{
    public class ItemsGUITab : RPGF.GUI.GUIElement
    {
        [SerializeField]
        private Image background;
        [SerializeField]
        private TextMeshProUGUI text;
        [Space]
        [SerializeField]
        private Color focusColor = Color.yellow;

        public override void OnFocused()
        {
            background.color = focusColor;
            text.color = focusColor;
        }

        public override void OnLostFocus()
        {
            background.color = Color.white;
            text.color = Color.white;
        }
    }
}
