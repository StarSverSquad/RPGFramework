using DG.Tweening;
using RPGF.Battle.Player;
using RPGF.Core;
using RPGF.Core.Battle;
using RPGF.Core.Battle.PlayerMode;
using RPGF.Domain.DI;
using UnityEngine;

namespace RPGFramework.RPGF.Battle
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

        private Tween ResizeTween = null;

        public override void Initialize()
        {
            gridRenderer = grid.GetComponent<SpriteRenderer>();

            _player.Mode.OnPlayerModeChanged += OnPlayerModeChanged;
            _field.OnResize += OnFieldResize;
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
                .Play();
        }
    }
}
