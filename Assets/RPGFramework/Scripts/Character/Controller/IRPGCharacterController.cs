using UnityEngine;

namespace RPGF.Character
{
    public enum Direction
    {
        Up, Down, Left, Right
    }

    /// <summary>
    /// Describe base API
    /// </summary>
    public interface IRPGCharacterController
    {
        public void RotateTo(Direction direction);

        public void MoveTo(Vector2 position, float time);
        public void MoveToRelative(Vector2 offset, float time);
    }
}