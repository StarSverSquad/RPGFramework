using RPGF.Core.Battle.Projectiles.Abstractions;
using UnityEngine;

namespace RPGF.Core.Battle.Projectiles
{
    public class BulletProjectile : ProjectileBase
    {
        [Header("Пуля:")]
        [SerializeField]
        [Range(0f, 360f * Mathf.Deg2Rad)]
        private float angle = 0f;
        public float Angle { get { return angle; } set { angle = value; } }
        [SerializeField]
        private float speed;
        public float Speed { get { return speed; } set { speed = value; } }

        protected Vector2 Direction => new(Mathf.Cos(angle), Mathf.Sin(angle));

        private void FixedUpdate()
        {
            transform.Translate(speed * Time.fixedDeltaTime * Direction.normalized);
        }

        private void OnDrawGizmosSelected()
        {
            Debug.DrawRay(transform.position, Direction.normalized, Color.red);
        }
    }
}
