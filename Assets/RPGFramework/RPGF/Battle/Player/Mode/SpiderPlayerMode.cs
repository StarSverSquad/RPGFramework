using DG.Tweening;
using RPGF.Battle.BattleField;
using RPGF.Core.Battle.BattleField;
using RPGF.Core.Battle.PlayerMode;
using RPGF.Domain.DI;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace RPGF.Battle.Player.Mode
{
    public class SpiderPlayerMode : PlayerModeBase
    {
        public const float LadderModeOffsetCorection = 0.125f;

        [Inject]
        private readonly BattleFieldManager _fields;

        public override PlayerModeEnum PlayerMode => PlayerModeEnum.Spider;
        public override Color SoulColor => Color.purple;

        [Header("Basic")]
        [SerializeField]
        private float verticalMoveTime = 0.25f;

        private int verticalOffset = 0;
        private SpiderBattleField battleField;
        private Tween verticalModeTween = null;
        private Coroutine ladderModeCoroutine = null;

        public override void Initialize()
        {
            battleField = _fields.OfType<SpiderBattleField>().FirstOrDefault();

            if (battleField != null)
            {
                battleField.OnLadderModeChangedCallback += OnLadderModeChangedHandler;
                UpdateVertical();
            }
            else
            {
                Debug.LogError("Spider battle field is not found!");
            }
        }

        private void Update()
        {
            if (Data.CanMove)
            {
                if (Input.GetKeyDown(Global.BaseOptions.MoveUp) && verticalOffset < battleField.MaxVerticalOffset)
                {
                    verticalOffset++;
                    UpdateVertical();
                }
                else if (Input.GetKeyDown(Global.BaseOptions.MoveDown) && verticalOffset > -battleField.MaxVerticalOffset)
                {
                    verticalOffset--;
                    UpdateVertical();
                }

                if (battleField.LadderMode && verticalOffset <= -battleField.MaxVerticalOffset)
                {
                    verticalOffset++;
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

                direction *= Data.MoveSpeed;

                if (battleField.LadderMode && verticalOffset > -battleField.MaxVerticalOffset)
                {
                    direction += Vector2.down * battleField.LadderModeSpeed;
                }

                transform.Translate(Time.fixedDeltaTime * direction, Space.Self);
            }
        }

        private void UpdateVertical()
        {
            verticalModeTween?.Kill();

            var newY = battleField.GetWebPoint(verticalOffset).position.y;

            if (battleField.LadderMode)
            {
                newY -= LadderModeOffsetCorection;
            }

            verticalModeTween = transform
                .DOLocalMoveY(newY, verticalMoveTime)
                .SetEase(Ease.Linear)
                .Play();
        }

        private void OnLadderModeChangedHandler(bool ladderMode)
        {
            if (!ladderMode)
            {
                UpdateVertical();  

                if (ladderModeCoroutine != null)
                {
                    StopCoroutine(ladderModeCoroutine);
                    ladderModeCoroutine = null;
                }
            }
            else
            {
                ladderModeCoroutine = StartCoroutine(LadderModeCoroutine());
            }
        }

        private IEnumerator LadderModeCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(SpiderBattleField.WebGap / battleField.LadderModeSpeed);

                if (verticalOffset > -battleField.MaxVerticalOffset)
                {
                    verticalOffset--;
                }
            }
        }

        public override void Dispose()
        {
            verticalOffset = 0;

            if (ladderModeCoroutine != null)
            {
                StopCoroutine(ladderModeCoroutine);
                ladderModeCoroutine = null;
            }

            battleField = null;
        }
    }
}
