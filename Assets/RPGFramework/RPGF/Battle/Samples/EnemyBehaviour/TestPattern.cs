using RPGF.Core.Battle;
using System.Collections;
using UnityEngine;

namespace RPGF.Battle.Samples.EnemyBehaviour
{
    public class TestPattern : BattleEnemyBehaviourBase
    {
        [SerializeField]
        private EnemyBulletBase SomeBullet;

        public int BulletsCount = 15;
        public float TimeOffset = 1f;

        protected override IEnumerator BehaviourCoroutine()
        {
            if (IsSingle)
                yield return MainPattern();
            else
                yield return TinyPatternCoroutine();
        }

        private IEnumerator MainPattern()
        {
            float time = Mathf.Abs(Time - TimeOffset) / (BulletsCount + 1);

            StartCoroutine(MoveField());

            while (true)
            {
                float y = Random.Range(-2.5f, 0.5f);

                CreateObjectRelativeCenter(SomeBullet.gameObject, new Vector2(-4, y));

                yield return new WaitForSeconds(time);
            }
        }

        private IEnumerator TinyPatternCoroutine()
        {
            float time = Mathf.Abs(Time - TimeOffset) / (BulletsCount + 1);

            while (true)
            {
                float y = Random.Range(-2.5f, 0.5f);

                CreateObjectRelativeCenter(SomeBullet.gameObject, new Vector2(-4, y));

                yield return new WaitForSeconds(time);
            }
        }

        private IEnumerator MoveField()
        {
            bool moveKey = false;

            while (true)
            {
                if (!Battle.BattleField.IsRotating)
                {
                    Battle.BattleField.Rotate(0);
                    Battle.BattleField.Rotate(359, 4);
                }

                if (!Battle.BattleField.IsMoving)
                {
                    if (moveKey)
                        Battle.BattleField.MoveRelative(new Vector2(-2, 0), 2);
                    else
                        Battle.BattleField.MoveRelative(new Vector2(2, 0), 2);

                    moveKey = !moveKey;
                }

                yield return null;
            }
        }
    }
}