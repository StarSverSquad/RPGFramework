using UnityEngine;

namespace RPGF.Battle.EnemyBehaviour.Sample
{
    public class DirectionalBullet : EnemyBehaviourBulletBase
    {
        public Vector2 Direction = Vector2.zero;

        private void FixedUpdate()
        {
            transform.Translate(Direction * Time.fixedDeltaTime);
        }
    }
}