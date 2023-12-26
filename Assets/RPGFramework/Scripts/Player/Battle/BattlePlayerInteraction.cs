using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattlePlayerInteraction : MonoBehaviour
{
    public float HitCooldown = 1f;

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private AudioSource audioSource;

    public bool IsHitCooldown => cooldownCorotine != null;

    private Coroutine cooldownCorotine;

    private void OnEnable()
    {
        cooldownCorotine = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PatternBullet")
        {
            PatternBullet bullet = collision.gameObject.GetComponent<PatternBullet>();

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
            animator.SetTrigger("DAMAGE");

            if (!IsHitCooldown)
                cooldownCorotine = StartCoroutine(CooldownCoroutine());

            foreach (var item in BattleManager.Data.Characters.Where(i => i.IsTarget))
            {
                BattleManager.Utility.DamageCharacterByBullet(item, bullet);
            }
        }
    }

    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(HitCooldown);

        cooldownCorotine = null;
    }
}
