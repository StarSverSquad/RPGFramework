using RPGF.Core.Battle.Abstractions;
using UnityEngine;

namespace RPGF.Battle.Samples.EnemyBullet
{
    public class DirectionalBullet : EnemyBulletBase
    {
        public Vector2 Direction = Vector2.zero;

        private void FixedUpdate()
        {
            transform.Translate(Direction * Time.fixedDeltaTime);
        }
    }
}