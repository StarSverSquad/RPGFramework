using RPGF.Domain.DI;
using RPGF.EventSystem;
using RPGF.EventSystem.Attributes;
using RPGF.Explorer;
using System.Collections;
using UnityEngine;

namespace RPGF.Actions
{
    [GenerateActionNode("Запуск события", "Запускает другое событие", "Прочее/Запуск события")]
    public class InvokeEventAction : ActionBase
    {
        [Inject]
        private readonly ExplorerEventHandler _eventHandler;

        [ActionFieldOption("Событие", AllowSceneObjects = true)]
        public LocationEvent Event;
        [ActionFieldOption("Ждать?")]
        public bool IsWait;

        public InvokeEventAction() : base()
        {
            Event = null;
            IsWait = false;
        }

        public override IEnumerator ActionCoroutine()
        {
            Event.InvokeEvent();

            if (IsWait)
            {
                if (_eventHandler.isActiveAndEnabled)
                    _eventHandler.ForceUnhandle();

                yield return new WaitWhile(() => Event.InnerEvent.IsPlaying);

                _eventHandler.HandleEvent(Event.InnerEvent);
            }
        }
    }
}