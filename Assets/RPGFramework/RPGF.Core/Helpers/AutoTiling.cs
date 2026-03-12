using UnityEngine;

namespace RPGF.Core.Helpers
{
    public class AutoTiling : RPGFrameworkBehaviour
    {
        [SerializeReference]
        private SpriteRenderer referenceRenderer;
        [SerializeField]
        private Vector2 margin;

        public void Tiling()
        {
            transform.localScale = referenceRenderer.size - (margin * 2);
        }
    }
}
