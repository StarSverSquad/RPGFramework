using RPGF.GUI;
using UnityEngine;
using UnityEngine.UI;

namespace GlackSaga.GUI.TittleMenu.CharactetSelector
{
    [RequireComponent(typeof(Image))]
    public class CharacterSelectorItem : GUIInteractable
    {
        [SerializeField]
        private Image image;

        public override void OnFocused()
        {
            image.enabled = true;
        }

        public override void OnUnfocused()
        {
            image.enabled = false;
        }
    }
}
