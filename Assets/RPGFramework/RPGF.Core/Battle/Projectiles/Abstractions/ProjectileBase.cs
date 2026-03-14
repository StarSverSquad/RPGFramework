using DG.Tweening;
using RPGF.RPG;
using System;
using UnityEngine;

namespace RPGF.Core.Battle.Projectiles.Abstractions
{
    public abstract class ProjectileBase : RPGFrameworkBehaviour, IDisposable
    {
        [Header("Настройки:")]
        public int DamageFactor = 1;
        public int ConcentrationAmount = 4;
        [Header("Интерации:")]
        public bool CanHitHalo = true;
        public bool DestroyAfterHit = true;
        public bool IgnoreHitCooldown = false;

        public RPGEntityState[] States;

        [HideInInspector]
        public RPGEnemy enemy;

        public virtual void SetHide(bool hide)
        {
            if (TryGetComponent<SpriteRenderer>(out var renderer))
            {
                if (hide)
                {
                    renderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                    renderer.renderingLayerMask = RenderingLayerMask.GetMask(RenderLayerMaskConstants.HiddenProjetilesLayerName);
                }
                else
                {
                    renderer.maskInteraction = SpriteMaskInteraction.None;
                    renderer.renderingLayerMask = RenderingLayerMask.GetMask(RenderLayerMaskConstants.DefaultLayerName);
                }
            }
        }

        public virtual void OnHit() { }
        public virtual void OnHitWhileCooldown() { }
        public virtual void OnHitHalo() { }

        /// <summary>
        /// Настоятельная просьба все tween очищать в рамках dispose
        /// </summary>
        public virtual void Dispose() { }
    }
}