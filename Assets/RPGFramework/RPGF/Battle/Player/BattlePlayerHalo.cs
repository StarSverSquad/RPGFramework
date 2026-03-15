using DG.Tweening;
using RPGF.Core;
using UnityEngine;
using RPGF.Core.Battle.Projectiles.Abstractions;
using System.Collections.Generic;
using System;
using System.Linq;

namespace RPGF.Battle.Player
{
    public class BattlePlayerHalo : RPGFrameworkBehaviour, IDisposable
    {
        private readonly List<int> hittedProjectilesIds = new();

        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private AudioSource audioSource;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(TagConstants.ProjectileTag))
            {
                if (hittedProjectilesIds.Any(i => i == collision.gameObject.GetInstanceID()))
                    return;

                var bullet = collision.gameObject.GetComponent<ProjectileBase>();

                if (!bullet.CanHitHalo)
                    return;

                hittedProjectilesIds.Add(collision.gameObject.GetInstanceID());

                bullet.OnHitHalo();

                audioSource.Play();

                spriteRenderer.DOColor(new Color(1, 1, 1, 0), 0.5f).From(Color.white).Play();

                Battle.Utility.AddConcetration(bullet.ConcentrationAmount);
            }
        }

        public void Dispose()
        {
            hittedProjectilesIds.Clear();
        }
    }
}