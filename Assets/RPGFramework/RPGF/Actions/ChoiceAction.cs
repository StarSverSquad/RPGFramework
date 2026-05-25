using RPGF.Core.Choice;
using RPGF.Domain.DI;
using RPGF.EventSystem;
using RPGF.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPGF.Actions
{
    [Serializable]
    public class ChoiceAction : ActionBase
    {
        [Inject]
        private readonly ChoiceDialogManager _choice = null!;

        [SerializeReference]
        public List<string> Choices;

        public int fallbackIndex;
        public bool canCancel;

        public ChoiceAction() : base()
        {
            Nexts.Clear();

            Choices = new List<string>();
            fallbackIndex = 0;
            canCancel = false;
        }

        public override IEnumerator ActionCoroutine()
        {
            _choice.SetCancelBlock(!canCancel);
            yield return _choice.Invoke(Choices.Select(i => new ChoiceItem { Label = i }));

            var resultIndex = _choice.State == ChoiceState.Canceled ? fallbackIndex : _choice.Index;

            SetNext($"Choice-{resultIndex}");
        }
    }
}
