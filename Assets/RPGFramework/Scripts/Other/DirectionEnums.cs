
using UnityEngine;

namespace RPGF
{
    public static class DirectionConverter
    {
        public static MoveDirection ViewDiretionToMoveDiretion(ViewDirection direction)
        {
            return (MoveDirection)direction;
        }

        public static ViewDirection MoveDiretionToViewDiretion(MoveDirection direction)
        {
            return (ViewDirection)direction;
        }

        public static MoveDirection GetMoveDiretionByVector(Vector2 vector)
        {
            if (vector.y > 0.7)
                return MoveDirection.Up;
            else if (vector.y < -0.7)
                return MoveDirection.Down;
            else if (vector.x > 0)
                return MoveDirection.Right;
            else if (vector.x < 0)
                return MoveDirection.Left;
            else
                return MoveDirection.None;
        }

        public static ViewDirection GetViewDirectionByVector(Vector2 vector)
        {
            if (vector.y > 0.70f)
                return ViewDirection.Up;
            else if (vector.y < 0.70f)
                return ViewDirection.Down;
            else if (vector.x > 0)
                return ViewDirection.Right;
            else if (vector.x < 0)
                return ViewDirection.Left;
            else
                return ViewDirection.Down;
        }
    }

    public enum ViewDirection
    {
        Up, Down, Left, Right
    }

    public enum MoveDirection
    {
        None = -1, Up, Down, Left, Right
    }
}