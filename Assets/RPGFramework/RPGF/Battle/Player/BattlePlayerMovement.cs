using RPGF.Core;
using UnityEngine;

namespace RPGF.Battle.Player
{
    public class BattlePlayerMovement : RPGFrameworkBehaviour
    {
        [SerializeField]
        private Rigidbody2D rb;

        public bool CanMove = true;

        public float DefaultMoveSpeed = 1;
        public float MoveSpeed = 1;

        private void FixedUpdate()
        {
            if (CanMove)
            {
                Vector2 direction = Vector2.zero;

                if (Input.GetKey(Game.BaseOptions.MoveUp))
                    direction += Vector2.up;
                if (Input.GetKey(Game.BaseOptions.MoveDown))
                    direction += Vector2.down;
                if (Input.GetKey(Game.BaseOptions.MoveLeft))
                    direction += Vector2.left;
                if (Input.GetKey(Game.BaseOptions.MoveRight))
                    direction += Vector2.right;

                rb.linearVelocity = direction.normalized * MoveSpeed;
            }
            else
                rb.linearVelocity = Vector2.zero;
        }
    }
}