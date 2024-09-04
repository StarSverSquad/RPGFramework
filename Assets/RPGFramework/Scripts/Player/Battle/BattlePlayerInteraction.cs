using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;

public class BattlePlayerInteraction : MonoBehaviour
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

            damageAnimation?.Kill();

            damageAnimation = DOTween.Sequence();

            damageAnimation.Append(
                spriteRenderer.DOColor(new Color(0.5f, 0, 0), 0.15f).From(Color.red));

            damageAnimation.Append(
                spriteRenderer.DOColor(Color.red, 0.15f));

            damageAnimation.SetLoops(3).Play();


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
