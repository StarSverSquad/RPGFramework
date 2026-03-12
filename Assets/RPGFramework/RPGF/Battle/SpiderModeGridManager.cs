using DG.Tweening;
using RPGF.Battle.Player;
using RPGF.Battle.Player.Mode;
using RPGF.Core;
using RPGF.Core.Battle.BattleField;
using RPGF.Core.Battle.PlayerMode;
using RPGF.Domain.DI;
using UnityEngine;

namespace RPGF.Battle
{
    public class SpiderModeGridManager : RPGFrameworkBehaviour
    {
        [Inject]
        private readonly BattlePlayerManager _player;
        [Inject]
        private readonly BattleFieldManager _field;

        [SerializeField]
        private Transform grid;
        private SpriteRenderer gridRenderer;
        [SerializeField]
        private SpriteRenderer playerModel;

        private SpiderPlayerMode modeHandler;

        private bool IsSpiderMode => _player.ModeManager.CurrentMode == PlayerModeEnum.Spider;
        private bool IsLadderMode => modeHandler.LadderMode;

        private Tween ResizeTween = null;
        private Tween RotateTween = null;


        public override void Initialize()
        {
            gridRenderer = grid.GetComponent<SpriteRenderer>();
            modeHandler = _player.ModeManager.GetMode<SpiderPlayerMode>(PlayerModeEnum.Spider);

            modeHandler.OnLadderModeChanged += OnLadderModeChanged;

            _player.ModeManager.OnPlayerModeChanged += OnPlayerModeChanged;

            //_field.OnResize += OnFieldResize;
            //_field.OnRortate += OnFieldRotate;
        }

        private void OnLadderModeChanged(bool isLadderMode)
        {

        }

        private void OnPlayerModeChanged(PlayerModeEnum mode)
        {
            gridRenderer.enabled = mode == PlayerModeEnum.Spider;
        }

        private void OnFieldResize(Vector2 size, float time, Ease ease)
        {
            ResizeTween?.Kill();

            ResizeTween = DOTween.To(
                () => gridRenderer.size,
                (value) => gridRenderer.size = value,
                size, time)
                .SetEase(ease)
                .Play();
        }

        private void OnFieldRotate(float arg1, float arg2, Ease arg3)
        {
            if (!IsSpiderMode)
                return;


        }
    }
}
