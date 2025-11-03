using RPGF.Domain.DI;
using RPGF.EventSystem;
using RPGF.Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGF.Actions
{
    public class ChoiceAction : ActionBase
    {
        [Inject]
        private readonly ChoiceBoxManager _choice;

        [SerializeReference]
        public List<string> Choices;

        public ChoiceBoxManager.Position Position;

        public Vector2 CustomPosition;

        public ChoiceAction() : base()
        {
            Nexts.Clear();

            Choices = new List<string>();
            CustomPosition = new Vector2();
            Position = ChoiceBoxManager.Position.Bottom;
        }

        public override IEnumerator ActionCoroutine()
        {
            _choice.ChangePosition(Position, CustomPosition);

            _choice.Choice(Choices.ToArray());

            yield return new WaitWhile(() => _choice.IsChoicing);

            SetNext($"choice-{_choice.Index}");
        }
    }
}