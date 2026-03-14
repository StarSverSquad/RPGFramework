using RPGF.Core.Battle.Projectiles.Abstractions;
using RPGF.Domain.DI;
using RPGF.RPG;
using System;
using System.Collections;
using UnityEngine;

namespace RPGF.Core.Battle.Abstractions
{
    public abstract class BattleEnemyBehaviourBase : RPGFrameworkBehaviour, IDisposable
    {
        private BattleEnemyBehaviourManager manager => Battle.EnemyBehaviour;       

        public string BehaviourTag = "";

        [Tooltip("Длительность во времни")]
        public float Time = 1f;
        [Tooltip("Ожидать завершения паттерна?")]
        public bool WaitEnd = false;

        public RPGEnemy Owner { get; private set; }

        public bool IsWorking => behaviourCoroutine != null;

        protected Vector2 BattleFieldPosition => Battle.BattleField.transform.position;
        protected bool IsSingle => Battle.EnemyBehaviour.BehaviourCount == 1;

        private Coroutine behaviourCoroutine = null;

        public override void Initialize()
        {
            Debug.LogWarning("Инициализация поведения не полная!");
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

        #region INNER API

        protected GameObject CreateObjectRelativeCenter(GameObject obj, Vector2 offset)
        {
            if (obj.TryGetComponent(out ProjectileBase pb))
                pb.enemy = Owner;

            return manager.CreateObjectRelativeCenter(obj, offset);
        }
        protected GameObject CreateObjectRelativeBattleField(GameObject obj, Vector2 offset)
        {
            if (obj.TryGetComponent(out ProjectileBase pb))
                pb.enemy = Owner;

            return manager.CreateObjectRelativeBattleField(obj, offset);
        }
        protected GameObject CreateObjectInWorldSpace(GameObject obj, Vector2 position)
        {
            if (obj.TryGetComponent(out ProjectileBase pb))
                pb.enemy = Owner;

            return manager.CreateObjectInWorldSpace(obj, position);
        }

        #endregion

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