using RPGF.Battle;
using RPGF.EventSystem;
using RPGF.Explorer;
using RPGF.RPG;
using System;
using System.Collections;
using UnityEngine;

namespace RPGF.Core.RPGEffect
{
    [Serializable]
    public class InvokeEventEffect : RPGEffectBase
    {
        public GlobalEvent @event;

        public override IEnumerator Invoke(RPGEntity user, RPGEntity target)
        {
            if (BattleManager.IsBattle)
            {
                if (@event != null)
                {
                    @event.InnerEvent.Invoke(BattleManager.Instance);

                    yield return new WaitWhile(() => @event.InnerEvent.IsPlaying);
                }
                else
                    Debug.LogError("Событие не указано!");
            }
            else
            {
                try
                {
                    if (@event == null)
                        throw new ApplicationException("Событие не указано!");

                    if (ExplorerManager.Instance.EventHandler.EventPlaying)
                        throw new ApplicationException("Не возможно запустить сразу два события!");

                    ExplorerManager.Instance.EventHandler.InvokeEvent(@event.InnerEvent);
                }
                catch (ApplicationException err)
                {
                    Debug.LogError(err.Message);
                }

                yield return new WaitWhile(() => @event.InnerEvent.IsPlaying);
            }
        }

        public override string GetName()
        {
            return "Запустить событие";
        }
    }
}