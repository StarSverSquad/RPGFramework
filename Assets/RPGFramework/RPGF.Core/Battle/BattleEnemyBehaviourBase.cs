using RPGF.Battle.Enemy;
using RPGF.Battle;
using RPGF.RPG;
using System;
using System.Collections;
using UnityEngine;

namespace RPGF.Core.Battle
{
    public abstract class BattleEnemyBehaviourBase : RPGFrameworkBehaviour, IDisposable
    {
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
            if (obj.TryGetComponent(out EnemyBulletBase pb))
                pb.enemy = Owner;

            return BattleManager.Instance.EnemyBehaviour.CreateObjectRelativeCenter(obj, offset);
        }
        protected GameObject CreateObjectRelativeBattleField(GameObject obj, Vector2 offset)
        {
            if (obj.TryGetComponent(out EnemyBulletBase pb))
                pb.enemy = Owner;

            return BattleManager.Instance.EnemyBehaviour.CreateObjectRelativeBattleField(obj, offset);
        }
        protected GameObject CreateObjectInWorldSpace(GameObject obj, Vector2 position)
        {
            if (obj.TryGetComponent(out EnemyBulletBase pb))
                pb.enemy = Owner;

            return BattleManager.Instance.EnemyBehaviour.CreateObjectInWorldSpace(obj, position);
        }

        protected BattleEnemyModel GetEnemyModel()
        {
            return Battle.EnemyModels.GetModel(Owner);
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