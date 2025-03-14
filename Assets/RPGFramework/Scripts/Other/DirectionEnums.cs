
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
            if (vector.y > 0.7f)
                return ViewDirection.Up;
            else if (vector.y < -0.7f)
                return ViewDirection.Down;
            else if (vector.x > 0)
                return ViewDirection.Right;
            else if (vector.x < 0)
                return ViewDirection.Left;
            else
                return ViewDirection.Down;
        }

        public static Vector2 GetVectorByViewDiretion(ViewDirection direction)
        {
            switch (direction)
            {
                case ViewDirection.Up:
                    return Vector2.up;
                case ViewDirection.Down:
                    return Vector2.down;
                case ViewDirection.Left:
                    return Vector2.left;
                case ViewDirection.Right:
                    return Vector2.right;
                default:
                    return Vector2.down;
            }
        }
        public static Vector2 GetVectorByMoveDiretion(MoveDirection direction)
        {
            switch (direction)
            {
                case MoveDirection.Up:
                    return Vector2.up;
                case MoveDirection.Down:
                    return Vector2.down;
                case MoveDirection.Left:
                    return Vector2.left;
                case MoveDirection.Right:
                    return Vector2.right;
                default:
                    return Vector2.zero;
            }
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