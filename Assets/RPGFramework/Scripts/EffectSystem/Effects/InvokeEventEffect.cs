using System;
using System.Collections;
using UnityEngine;

public class InvokeEventEffect : EffectBase
{
    public GraphEvent @event;

    public override IEnumerator Invoke(RPGEntity user, RPGEntity target)
    {
        if (BattleManager.IsBattle)
        {
            if (@event != null)
            {
                @event.Invoke(BattleManager.Instance);

                yield return new WaitWhile(() => @event.IsPlaying);
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

                if (ExplorerManager.Instance.EventHandler.EventRuning)
                    throw new ApplicationException("Не возможно запустить сразу два события!");

                ExplorerManager.Instance.EventHandler.InvokeEvent(@event);
            }
            catch (ApplicationException err)
            {
                Debug.LogError(err.Message);
            }

            yield return new WaitWhile(() => @event.IsPlaying);
        }
    }

    public override string GetName()
    {
        return "Запустить событие";
    }
}