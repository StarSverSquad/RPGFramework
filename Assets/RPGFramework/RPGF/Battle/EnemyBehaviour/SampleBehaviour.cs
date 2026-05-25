using RPGF.Core.Battle.BattleField;
using RPGF.Core.Battle.Behaviour.Abstractions;
using RPGF.Core.Battle.Projectiles;
using RPGF.Domain.DI;
using System.Collections;
using UnityEngine;

namespace RPGF.Battle.EnemyBehaviour
{
    public class SampleBehaviour : BattleEnemyBehaviourBase
    {
        [Inject]
        private readonly ProjectileManager _projectiles = null!;
        [Inject]
        private readonly BattleFieldManager _field = null!;

        [SerializeField]
        private BulletProjectile bullet;
        [SerializeField]
        private int projectileSpawnRate = 8;

        protected override IEnumerator BehaviourCoroutine()
        {
            var field = _field.MainField;
            field.Resize(new Vector2(4, 4));

            bool isRight = true;
            while (true)
            {
                Vector2 position = 
                    isRight ? 
                    new(field.Size.x + 3f, Random.Range(-field.Size.y / 2f, field.Size.y / 2f)) :
                    new(-field.Size.x - 3f, Random.Range(-field.Size.y / 2f, field.Size.y / 2f));

                var angle = isRight ? 180f * Mathf.Deg2Rad : 0f;

                var projectile = _projectiles.Create(bullet, position, Owner);
                projectile.Angle = angle;

                isRight = !isRight;

                yield return new WaitForSeconds(1f / projectileSpawnRate);
            }
        }
    }
}
