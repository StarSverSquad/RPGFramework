using RPGF.Core.Battle.PlayerMode;
using UnityEngine;

namespace RPGF.Battle.Player.Mode
{
    public class SoulPlayerMode : PlayerModeBase
    {
        public override PlayerModeEnum PlayerMode => PlayerModeEnum.Soul;

        public override Color SoulColor => Color.gold;

        private void FixedUpdate()
        {
            if (Data.CanMove)
            {
                Vector2 direction = Vector2.zero;

                if (Input.GetKey(Global.BaseOptions.MoveUp))
                    direction += Vector2.up;
                if (Input.GetKey(Global.BaseOptions.MoveDown))
                    direction += Vector2.down;
                if (Input.GetKey(Global.BaseOptions.MoveLeft))
                    direction += Vector2.left;
                if (Input.GetKey(Global.BaseOptions.MoveRight))
                    direction += Vector2.right;

                Data.Rigidbody.linearVelocity = direction.normalized * Data.MoveSpeed;
            }
            else
                Data.Rigidbody.linearVelocity = Vector2.zero;
        }
    }
}