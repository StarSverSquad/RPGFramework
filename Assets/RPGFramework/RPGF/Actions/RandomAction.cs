using RPGF.EventSystem;
using System.Collections;
using UnityEngine;

namespace RPGF.Actions
{
    public class RandomAction : ActionBase
    {
        public const string YES_NextTag = "YES";
        public const string NO_NextTag = "NO";

        public float Chance;

        public RandomAction() : base()
        {
            Nexts.Clear();

            AddNext(YES_NextTag);
            AddNext(NO_NextTag);

            Chance = 0.5f;
        }

        public override IEnumerator ActionCoroutine()
        {
            float result = Random.Range(0f, 1f);

            if (result > Chance)
            {
                SetNext(YES_NextTag);
            }
            else
            {
                SetNext(NO_NextTag);
            }

            yield break;
        }
    }
}