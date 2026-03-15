using DG.Tweening;
using RPGF.Battle.BattleField;
using RPGF.Battle.Player;
using RPGF.Core.Battle.BattleField;
using RPGF.Core.Battle.Behaviour.Abstractions;
using RPGF.Core.Battle.Projectiles;
using RPGF.Domain.DI;
using System.Collections;
using UnityEngine;

namespace RPGF.Battle.EnemyBehaviour
{
    public class TestSpiderBehaviour : BattleEnemyBehaviourBase
    {
        [Inject]
        private readonly ProjectileManager _projectiles;
        [Inject]
        private readonly BattlePlayerManager _player;
        [Inject]
        private readonly BattleFieldManager _fields;

        [SerializeField]
        private BulletProjectile bullet;
        [SerializeField]
        private SpiderBattleField battleField;

        [SerializeField]
        private int projectilesPerSecound = 4;

        private Sequence rotateSequence;

        protected override IEnumerator BehaviourCoroutine()
        {
            var field = _fields.Create(battleField);
            field.Resize(new Vector2(field.Size.x, 4.3f), 1);
            field.SetVerticalOffset(3);
            field.Show();

            _player.ModeManager.SetMode(Core.Battle.PlayerMode.PlayerModeEnum.Spider);

            field.LadderMode = true;

            rotateSequence = DOTween.Sequence();

            rotateSequence.Append(_fields.transform
                .DORotateQuaternion(Quaternion.Euler(0, 0, 7), 1.5f)
                .From(Quaternion.Euler(0, 0, -7))
                .SetEase(Ease.InOutSine));

            rotateSequence.Append(_fields.transform
                .DORotateQuaternion(Quaternion.Euler(0, 0, -7), 1.5f)
                .SetEase(Ease.InOutSine));

            rotateSequence.SetLoops(-1).Play();

            while (true)
            {
                _projectiles.Create(bullet, new(-4, 0), Owner);

                yield return new WaitForSeconds(1f / projectilesPerSecound);
            }
        }

        public override void Dispose()
        {
            rotateSequence.Kill();
            rotateSequence = null;
            _fields.transform.rotation = Quaternion.identity;
        }
    }
}