using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using RPGF.RPG;
using RPGF.Domain.DI;
using RPGF.Core.Battle.BattleField;
using RPGF.Core.Battle.Behaviour.Abstractions;
using System;

namespace RPGF.Core.Battle.Behaviour
{
    public class BattleEnemyBehaviourManager : RPGFrameworkBehaviour, IDisposable
    {
        [Inject]
        private readonly DependencyInjection _di = null!;

        private readonly List<BattleEnemyBehaviourBase> _behaviours = new();

        [SerializeField]
        private float waitWithoutBehaviours = 2f;

        public int BehaviourCount => _behaviours.Count;
        public bool IsWorking => behaviourControllCoroutine != null;
        public bool BattleFieldRequired => _behaviours.Any(i => i.BattleFieldRequired);

        private Coroutine behaviourControllCoroutine = null;

        public void AddBehaviour(BattleEnemyBehaviourBase behaviour, RPGEnemy owner)
        {
            var obj = Instantiate(behaviour.gameObject, transform, false);
            var useBehaviour = obj.GetComponent<BattleEnemyBehaviourBase>();

            _di.InjectInto(useBehaviour);
            useBehaviour.Initialize(owner);

            _behaviours.Add(useBehaviour);
        }

        public void Invoke()
        {
            if (IsWorking)
                return;

            transform.position = (Vector2)Camera.main.transform.position;

            foreach (var behaviour in _behaviours)
            {
                _di.InjectInto(behaviour);
                behaviour.Invoke();
            }

            behaviourControllCoroutine = StartCoroutine(BehavioursControllCoroutine());
        }

        public void Break()
        {
            foreach (BattleEnemyBehaviourBase b in _behaviours)
                b.Dispose();

            StopBehaviourControllCoroutine();
            Dispose();
        }

        private void StopBehaviourControllCoroutine()
        {
            if (behaviourControllCoroutine != null)
            {
                StopCoroutine(behaviourControllCoroutine);
                behaviourControllCoroutine = null;
            }
        }

        private IEnumerator BehavioursControllCoroutine()
        {
            if (_behaviours.Count > 0)
            {
                float maxTime = _behaviours.Max(i => i.Time);
                yield return new WaitForSeconds(maxTime);

                if (_behaviours.Any(i => i.WaitEnd))
                {
                    yield return new WaitUntil(() => _behaviours.All(i => !i.IsWorking));
                }
            }
            else
            {
                yield return new WaitForSeconds(waitWithoutBehaviours);
            }

            Dispose();
            behaviourControllCoroutine = null;
        }

        public void Dispose()
        {
            foreach (var beh in _behaviours)
            {
                beh.Dispose();
                Destroy(beh.gameObject);
            }
            _behaviours.Clear();
        }
    }
}
