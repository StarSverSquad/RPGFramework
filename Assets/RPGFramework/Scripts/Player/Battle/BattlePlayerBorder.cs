using DG.Tweening;
using UnityEngine;

public class BattlePlayerBorder : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PatternBullet")
        {
            PatternBullet bullet = collision.gameObject.GetComponent<PatternBullet>();

            if (bullet.IsHitBorder)
                return;

            bullet.IsHitBorder = true;

            bullet.OnHitBorder();

            audioSource.Play();

            spriteRenderer.DOColor(new Color(1, 1, 1, 0), 0.5f).From(Color.white).Play();

            BattleManager.Utility.AddConcetration(bullet.AdditionConcentration);
        }
    }
}