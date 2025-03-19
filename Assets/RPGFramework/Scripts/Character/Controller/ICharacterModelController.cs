using UnityEngine;

namespace RPGF.Character
{
    /// <summary>
    /// Describe base API
    /// </summary>
    public interface ICharacterModelController
    {
        public void RotateTo(ViewDirection direction);

        public void MoveTo(Vector2 position, float time);
        public void MoveToRelative(Vector2 offset, float time);
    }
}