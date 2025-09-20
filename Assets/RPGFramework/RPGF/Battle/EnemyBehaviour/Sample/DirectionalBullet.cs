using UnityEngine;

namespace RPGF.Battle.EnemyBehaviour.Sample
{
    public class DirectionalBullet : EnemyBehaviourBulletBase
    {
        [SerializeField]
        private GameObject model;
        public GameObject Model => model;

        public Vector2 Direction = Vector2.zero;

        private void FixedUpdate()
        {
            transform.Translate(Direction * Time.fixedDeltaTime);
        }
    }
}