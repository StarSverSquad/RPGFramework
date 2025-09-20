using RPGF.Core;
using RPGF.GUI.Other;
using RPGF.RPG;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPGF.Battle.Enemy
{
    public class BattleEnemyModel : RPGFrameworkBehaviour, IDisposable
    {
        [Serializable]
        public struct ModelAnimator
        {
            public Animator Animator;
            public string Tag;
        }

        [Header("Ссылки")]
        [SerializeField]
        private GameObject modelContainer;
        [SerializeField]
        private IconList iconList;

        [SerializeField]
        private RectTransform damageTxtPoint;
        [SerializeField]
        private RectTransform attackPoint;

        [Header("Еффекты")]
        [SerializeField,]
        private VisualEffectBase deathEffect;
        [SerializeField]
        private VisualEffectBase damageEffect;

        [Header("Аниматоры")]
        [SerializeField]
        private List<ModelAnimator> animators = new List<ModelAnimator>();

        public Vector2 DamageTextWorldPoint => damageTxtPoint.transform.position;
        public Vector2 AttackWorldPoint => attackPoint.transform.position;
        public bool IsAnyEffectPlaying => damageEffect.IsPlaying || deathEffect.IsPlaying;

        private RPGEnemy enemy;
        private bool isInit = false;

        public override void Initialize()
        {
            Debug.LogError("EnemyModel.Initialize() is not implemented");
        }
        public void Initialize(RPGEnemy enemy)
        {
            this.enemy = enemy;

            enemy.OnStateChanged += UpdateStats;

            UpdateStats(null);

            isInit = true;
        }

        #region API

        public void UpdateStats(RPGEntityState state)
        {
            var icons = enemy.States.Select(i => i.Icon).ToArray();
            iconList.UpdateIcons(icons);
        }
        public void Damage()
        {
            damageEffect?.Play();
        }
        public void Death()
        {
            deathEffect?.Play();
        }

        #endregion

        public void Dispose()
        {
            if (damageEffect != null)
                damageEffect.Dispose();

            if (deathEffect != null)
                deathEffect.Dispose();
        }

        public Animator GetAnimator(string tag)
        {
            return animators.First(a => a.Tag == tag).Animator;
        }

        private void OnDestroy()
        {
            if (isInit)
            {
                enemy.OnStateChanged -= UpdateStats;
                isInit = false;
            }
        }
    }
}