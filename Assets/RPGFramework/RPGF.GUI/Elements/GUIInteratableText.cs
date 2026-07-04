using TMPro;
using UnityEngine;

namespace RPGF.GUI.Elements
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class GUIInteratableText : GUIInteractable
    {
        protected TextMeshProUGUI textMesh;

        [SerializeField]
        protected Color focusColor = Color.gold;
        [SerializeField]
        protected Color unfocusColor = Color.white;

        private void OnEnable()
        {
            if (textMesh == null)
            {
                textMesh = GetComponent<TextMeshProUGUI>();
            }
        }

        public override void OnFocused()
        {
            base.OnFocused();

            textMesh.color = focusColor;
        }

        public override void OnUnfocused()
        {
            base.OnUnfocused();

            textMesh.color = unfocusColor;
        }
    }
}
