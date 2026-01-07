using RPGF.Core;
using RPGF.GUI;
using UnityEngine;

namespace RPGF.GUI
{
    [RequireComponent(typeof(RectTransform))]
    public class GUIElementBase : RPGFrameworkBehaviour, IGUIElement
    {
        private RectTransform rectTransform;
        public RectTransform RectTransform => rectTransform;

        public override void Initialize()
        {
            base.Initialize();

            rectTransform = GetComponent<RectTransform>();
        }

        public virtual void Dispose() { }
    }
}
