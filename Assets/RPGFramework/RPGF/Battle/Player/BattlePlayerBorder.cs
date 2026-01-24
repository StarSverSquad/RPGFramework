using DG.Tweening;
using RPGF.Battle.EnemyBehaviour;
using RPGF.Core;
using UnityEngine;

namespace RPGF.Battle.Player
{
    public class BattlePlayerBorder : RPGFrameworkBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private AudioSource audioSource;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("PatternBullet"))
            {
                var bullet = collision.gameObject.GetComponent<EnemyBehaviourBulletBase>();

                if (bullet.IsHitBorder)
                    return;

                bullet.IsHitBorder = true;

                bullet.OnHitBorder();

                audioSource.Play();

                spriteRenderer.DOColor(new Color(1, 1, 1, 0), 0.5f).From(Color.white).Play();

                Battle.Utility.AddConcetration(bullet.AdditionConcentration);
            }
        }
    }
}