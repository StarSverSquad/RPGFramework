using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BattlePlayerBorder : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
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
            animator.SetTrigger("BLINK");

            /// TODO

            BattleManager.Utility.AddConcetration(bullet.AdditionConcentration);
        }
    }
}