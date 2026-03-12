using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using RPGF.RPG;
using RPGF.Core.Battle.Abstractions;
using RPGF.Domain.DI;
using RPGF.Core.Battle.BattleField;

namespace RPGF.Core.Battle
{
    public class BattleEnemyBehaviourManager : RPGFrameworkBehaviour
    {
        [Inject]
        private readonly BattleFieldManager _field;
        [Inject]
        private readonly DependencyInjection DI;

        private readonly List<GameObject> behavioursObjects = new();
        private readonly List<BattleEnemyBehaviourBase> behaviours = new();

        [SerializeField]
        private float waitWithoutBehaviours = 2f;

        public int BehaviourCount => behaviours.Count;
        public bool IsWorking => attackCoroutine != null;


        private Coroutine attackCoroutine = null;

        public void AddBehaviour(BattleEnemyBehaviourBase behaviour, RPGEnemy owner)
        {
            var obj = Instantiate(behaviour.gameObject, transform, false);
            var useBehaviour = obj.GetComponent<BattleEnemyBehaviourBase>();

            useBehaviour.Initialize(owner);

            behaviours.Add(useBehaviour);
        }

        public void Invoke()
        {
            if (IsWorking)
                return;

            transform.position = (Vector2)Camera.main.transform.position;

            foreach (var behaviour in behaviours)
            {
                DI.InjectInto(behaviour);
                behaviour.Invoke();
            }
                

            attackCoroutine = StartCoroutine(BehavioursControllCoroutine());
        }
        public void Break()
        {
            foreach (BattleEnemyBehaviourBase b in behaviours)
                b.Dispose();

            StopAttackCoroutine();

            CleanUp();
        }

        public GameObject CreateObjectRelativeCenter(GameObject obj, Vector2 offset)
        {
            GameObject o = Instantiate(obj, (Vector2)transform.position + offset, Quaternion.identity, transform);
            behavioursObjects.Add(o);

            return o;
        }
        public GameObject CreateObjectRelativeBattleField(GameObject obj, Vector2 offset)
        {
            GameObject o = Instantiate(obj, (Vector2)_field.transform.position + offset, Quaternion.identity, transform);

            behavioursObjects.Add(o);

            return o;
        }
        public GameObject CreateObjectInWorldSpace(GameObject obj, Vector2 position)
        {
            GameObject o = Instantiate(obj, position, Quaternion.identity, transform);

            behavioursObjects.Add(o);

            return o;
        }

        public void CleanUp()
        {
            foreach (var o in behavioursObjects)
            {
                if (o != null)
                    Destroy(o);
            }
            behavioursObjects.Clear();

            foreach (var beh in behaviours)
            {
                beh.Dispose();
                Destroy(beh.gameObject);
            }
            behaviours.Clear();
        }

        private void StopAttackCoroutine()
        {
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }

        private IEnumerator BehavioursControllCoroutine()
        {
            if (behaviours.Count > 0)
            {
                float maxTime = behaviours.Max(i => i.Time);
                yield return new WaitForSeconds(maxTime);

                if (behaviours.Any(i => i.WaitEnd))
                {
                    yield return new WaitUntil(() => behaviours.All(i => !i.IsWorking));
                }
            }
            else
            {
                yield return new WaitForSeconds(waitWithoutBehaviours);
            }

            CleanUp();
            attackCoroutine = null;
        }
    }
}