using DG.Tweening;
using RPGF.Battle.EnemyBehaviour;
using RPGF.Core;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace RPGF.Battle.Player
{
    public class BattlePlayerInteraction : RPGFrameworkBehaviour
    {
        public float HitCooldown = 1f;

        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public bool IsHitCooldown => cooldownCorotine != null;

        private Coroutine cooldownCorotine;

        private Sequence damageAnimation;

        private void OnEnable()
        {
            cooldownCorotine = null;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("PatternBullet"))
            {
                var bullet = collision.gameObject.GetComponent<EnemyBehaviourBulletBase>();

                if (IsHitCooldown && !bullet.IgnoreHitCooldown)
                {
                    bullet.OnCooldownHit();

                    if (bullet.DestroyAfterHit)
                        Destroy(bullet.gameObject);

                    return;
                }

                bullet.OnHit();

                if (bullet.DestroyAfterHit)
                    Destroy(bullet.gameObject);

                audioSource.Play();

                damageAnimation?.Kill();

                damageAnimation = DOTween.Sequence();

                damageAnimation.Append(
                    spriteRenderer.DOColor(new Color(0.5f, 0.5f, 0.5f), 0.15f).From(Color.white));

                damageAnimation.Append(
                    spriteRenderer.DOColor(Color.white, 0.15f));

                damageAnimation.SetLoops(3).Play();

                if (!IsHitCooldown)
                    cooldownCorotine = StartCoroutine(CooldownCoroutine());

                foreach (var item in Battle.Data.TurnsData.Where(i => i.IsTarget))
                {
                    Battle.Utility.DamageCharacterByBullet(item, bullet);
                }
            }
        }

        private IEnumerator CooldownCoroutine()
        {
            yield return new WaitForSeconds(HitCooldown);

            cooldownCorotine = null;
        }
    }

}