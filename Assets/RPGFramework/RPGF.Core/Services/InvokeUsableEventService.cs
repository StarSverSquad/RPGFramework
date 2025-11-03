using RPGF.Battle;
using RPGF.Domain.DI;
using RPGF.Domain.Interfaces;
using RPGF.Explorer;
using RPGF.RPG;
using System.Collections;
using UnityEngine;

namespace RPGF.Core.Services
{
    public class InvokeUsableEventService : IService
    {
        [Inject]
        private readonly ExplorerEventHandler _eventHandler;
        [Inject]
        private readonly DependencyInjection _di;
        [Inject]
        private readonly BattleManager _battleManager;

        public void InvokeEvent(RPGUsable usable)
        {
            if (usable.Event != null)
            {
                switch (usable.Usage)
                {
                    case Usability.Any:
                        if (BattleManager.IsBattle)
                            usable.Event.Invoke(_battleManager, _di);
                        else if (!_eventHandler.EventPlaying)
                        {
                            usable.Event.Invoke(_eventHandler, _di);
                            _eventHandler.HandleEvent(usable.Event.InnerEvent);
                        }
                        break;
                    case Usability.Battle:
                        if (BattleManager.IsBattle)
                            usable.Event.Invoke(_battleManager, _di);
                        break;
                    case Usability.Explorer:
                        if (!BattleManager.IsBattle && !_eventHandler.EventPlaying)
                        {
                            usable.Event.Invoke(_eventHandler, _di);
                            _eventHandler.HandleEvent(usable.Event.InnerEvent);
                        }
                        break;
                }
            }
        }

        public IEnumerable WaitInvokeEvent(RPGUsable usable)
        {
            InvokeEvent(usable);

            yield return new WaitWhile(() => usable.Event.IsPlaying);
        }
    }
}
