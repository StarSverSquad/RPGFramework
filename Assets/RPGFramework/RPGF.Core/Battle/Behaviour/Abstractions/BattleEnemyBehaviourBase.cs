using RPGF.RPG;
using System;
using System.Collections;
using UnityEngine;

namespace RPGF.Core.Battle.Behaviour.Abstractions
{
    public abstract class BattleEnemyBehaviourBase : RPGFrameworkBehaviour, IDisposable
    {
        [SerializeField]
        private string behaviourTag;
        public string BehaviourTag => behaviourTag;

        [Tooltip("Длительность во времни")]
        public float Time = 1f;
        [Tooltip("Ожидать завершения паттерна?")]
        public bool WaitEnd = false;
        [Tooltip("Необходимо поле битвы?")]
        public bool BattleFieldRequired = true;

        public RPGEnemy Owner { get; private set; }

        public bool IsWorking => behaviourCoroutine != null;

        private Coroutine behaviourCoroutine = null;

        public override void Initialize()
        {
            Debug.LogError("Using Initialize(RPGEnemy owner) instead of Initialize()");
        }
        public void Initialize(RPGEnemy owner)
        {
            Owner = owner;
        }

        public void Invoke()
        {
            StopBehaviourCoroutine();
            StartCoroutine(MainPatternCoroutine());
        }

        protected abstract IEnumerator BehaviourCoroutine();

        private IEnumerator MainPatternCoroutine()
        {
            yield return BehaviourCoroutine();

            behaviourCoroutine = null;
        }

        private void StopBehaviourCoroutine()
        {
            if (IsWorking)
            {
                StopCoroutine(behaviourCoroutine);
                behaviourCoroutine = null;
            }
        }

        public virtual void Dispose()
        {
            StopBehaviourCoroutine();
        }
    }
}