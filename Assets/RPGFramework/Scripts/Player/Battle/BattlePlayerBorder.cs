using DG.Tweening;
using RPGF.Battle.Pattern;
using UnityEngine;

public class BattlePlayerBorder : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PatternBulletBase")
        {
            PatternBulletBase bullet = collision.gameObject.GetComponent<PatternBulletBase>();

            if (bullet.IsHitBorder)
                return;

            bullet.IsHitBorder = true;

            bullet.OnHitBorder();

            audioSource.Play();

            spriteRenderer.DOColor(new Color(1, 1, 1, 0), 0.5f).From(Color.white).Play();

            BattleManager.BattleUtility.AddConcetration(bullet.AdditionConcentration);
        }
    }
}