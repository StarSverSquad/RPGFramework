using RPGF.Domain.DI;
using RPGF.EventSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPGF.Actions.Condition
{
    public class ConditionAction : ActionBase
    {
        [Inject]
        private readonly DependencyInjection _di = null!;

        public const string NextThenTag = "Then";
        public const string NextElseTag = "Else";

        [SerializeReference]
        public List<ConditionBase> Conditions;

        public ConditionAction() : base()
        {
            Nexts.Clear();
            AddNext(NextThenTag, "Тогда");
            AddNext(NextElseTag, "Иначе");

            Conditions = new();
        }

        public override IEnumerator ActionCoroutine()
        {
            Conditions.ForEach(c => _di.InjectInto(c));

            bool isRight = Conditions.Any(c => c.Invoke());

            if (isRight)
                SetNext(NextThenTag);
            else
                SetNext(NextElseTag);

            yield break;
        }
    }

    /// <summary>
    /// Операции для события сравнения
    /// </summary>
    public enum ConditionOperation
    {
        Equals, NotEquals, More, Less, MoreOrEquals, LessOrEquals
    }
}
