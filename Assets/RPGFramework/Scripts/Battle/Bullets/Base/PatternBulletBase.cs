using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace RPGF.Battle.Pattern
{
    public class PatternBulletBase : RPGFrameworkBehaviour, IDisposable
    {
        public int DamageModifier = 1;

        public int AdditionConcentration = 4;

        public bool CanHitBorder = true;
        public bool DestroyAfterHit = true;
        public bool IgnoreHitCooldown = false;

        public RPGEntityState[] States;

        [HideInInspector]
        public RPGEnemy enemy;

        public bool IsHitBorder { get; set; } = false;

        protected void DisposeAllTweens()
        {
            foreach (var component in gameObject.GetComponents<Component>())
                component.DOKill(false);
        }

        public void Dispose()
        {
            DisposeAllTweens();
        }

        #region VIRTUALS

        public virtual void OnHit() { }
        public virtual void OnCooldownHit() { }
        public virtual void OnHitBorder() { }

        #endregion

        protected virtual void OnDisable()
        {
            DisposeAllTweens();
        }
        protected virtual void OnDestroy()
        {
            DisposeAllTweens();
        }
    }
}