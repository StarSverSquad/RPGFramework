using DG.Tweening;
using RPGF.Core;
using System.Collections;
using System.Linq;
using UnityEngine;
using RPGF.Core.Battle.Projectiles.Abstractions;

namespace RPGF.Battle.Player
{
    public class BattlePlayerInteraction : RPGFrameworkBehaviour
    {
        public float HitCooldown = 1f;

        [SerializeField]
        private AudioSource hurtSound;
        [SerializeField]
        private SpriteRenderer hurtMask;

        public bool IsHitCooldown => cooldownCorotine != null;

        private Coroutine cooldownCorotine;
        private Sequence damageAnimation;

        private void OnEnable()
        {
            cooldownCorotine = null;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(TagConstants.ProjectileTag))
            {
                var projectile = collision.gameObject.GetComponent<ProjectileBase>();

                if (IsHitCooldown && !projectile.IgnoreHitCooldown)
                {
                    projectile.OnHitWhileCooldown();

                    if (projectile.DestroyAfterHit)
                    {
                        projectile.Dispose();
                        Destroy(projectile.gameObject);
                    }

                    return;
                }

                projectile.OnHit();

                if (projectile.DestroyAfterHit)
                    Destroy(projectile.gameObject);

                hurtSound.Play();

                AnimateDamage();

                if (!IsHitCooldown)
                    cooldownCorotine = StartCoroutine(CooldownCoroutine());

                foreach (var item in Battle.Data.TurnsData.Where(i => i.IsTarget))
                {
                    Battle.Utility.DamageCharacterByProjectile(item, projectile);
                }
            }
        }

        private void AnimateDamage()
        {
            damageAnimation?.Kill();

            damageAnimation = DOTween.Sequence();

            damageAnimation.Append(
                hurtMask.DOColor(new Color(0, 0, 0, 0.5f), 0.15f).From(new Color(0, 0, 0, 0)));

            damageAnimation.Append(
                hurtMask.DOColor(new Color(0, 0, 0, 0), 0.15f));

            damageAnimation.SetLoops(3).Play();
        }

        private IEnumerator CooldownCoroutine()
        {
            yield return new WaitForSeconds(HitCooldown);

            cooldownCorotine = null;
        }
    }

}