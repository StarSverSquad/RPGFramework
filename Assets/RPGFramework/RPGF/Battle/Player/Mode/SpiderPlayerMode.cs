using DG.Tweening;
using RPGF.Core.Battle.PlayerMode;
using UnityEngine;

namespace RPGF.Battle.Player.Mode
{
    public class SpiderPlayerMode : PlayerModeBase
    {
        public override PlayerModeEnum PlayerMode => PlayerModeEnum.Spider;
        public override Color SoulColor => Color.purple;

        public int MaxVerticalOffset { get; set; }

        [SerializeField]
        private float verticalMoveTime = 0.25f;
        [SerializeField]
        private int initialMaxVerticalOffset = 1;

        private int verticalOffset = 0;
        private Tween verticalModeTween = null;

        public override void Initialize()
        {
            MaxVerticalOffset = initialMaxVerticalOffset;
        }

        private void Update()
        {
            if (Data.CanMove)
            {
                if (Input.GetKeyDown(Global.BaseOptions.MoveUp) && verticalOffset < MaxVerticalOffset)
                {
                    verticalOffset++;
                    UpdateVertical();
                }
                else if (Input.GetKeyDown(Global.BaseOptions.MoveDown) && verticalOffset > -MaxVerticalOffset)
                {
                    verticalOffset--;
                    UpdateVertical();
                }
            }
        }

        private void FixedUpdate()
        {    
            if (Data.CanMove)
            {
                Vector2 direction = Vector2.zero;

                if (Input.GetKey(Global.BaseOptions.MoveLeft))
                    direction += Vector2.left;
                if (Input.GetKey(Global.BaseOptions.MoveRight))
                    direction += Vector2.right;

                Data.Rigidbody.linearVelocity = direction.normalized * Data.MoveSpeed;
            }
            else
                Data.Rigidbody.linearVelocity = Vector2.zero;
        }

        private void UpdateVertical()
        {
            verticalModeTween?.Kill();

            verticalModeTween = transform
                .DOMoveY(Battle.BattleField.transform.position.y + verticalOffset, verticalMoveTime)
                .SetEase(Ease.Linear)
                .Play();
        }

        public override void Dispose()
        {
            verticalOffset = 0;
            MaxVerticalOffset = initialMaxVerticalOffset;
        }
    }
}
