using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using RPGF.Core;
using RPGF.Core.Battle.Projectiles.Abstractions;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

namespace RPGF.Battle.Player
{
    public class BattlePlayerHalo : RPGFrameworkBehaviour, IDisposable
    {
        private readonly HashSet<EntityId> hitProjectilesIds = new();

        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private AudioSource audioSource;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(TagConstants.ProjectileTag))
            {
                if (hitProjectilesIds.Contains(collision.gameObject.GetEntityId()))
                    return;

                var bullet = collision.gameObject.GetComponent<ProjectileBase>();

                if (!bullet.CanHitHalo)
                    return;

                hitProjectilesIds.Add(collision.gameObject.GetEntityId());

                bullet.OnHitHalo();

                audioSource.Play();

                spriteRenderer.DOColor(new Color(1, 1, 1, 0), 0.5f).From(Color.white).Play();

                Battle.Utility.AddConcetration(bullet.ConcentrationAmount);
            }
        }

        public void Dispose()
        {
            hitProjectilesIds.Clear();
        }
    }
}