using System;
using System.Collections;
using UnityEngine;

public class InvokeEventEffect : EffectBase
{
    public GraphEvent @event;

    public override IEnumerator BattleInvoke(BattleEntityInfo user, BattleEntityInfo target)
    {
        if (@event != null)
        {
            @event.Invoke(BattleManager.instance.pipeline);

            yield return new WaitWhile(() => @event.IsPlaying);
        }
        else
            Debug.LogError("Событие не указано!");
    }

    public override IEnumerator ExplorerInvoke(RPGEntity user, RPGEntity target)
    {
        try
        {
            if (@event == null)
                throw new ApplicationException("Событие не указано!");

            if (ExplorerManager.instance.eventHandler.EventRuning)
                throw new ApplicationException("Не возможно запустить сразу два события!");

            ExplorerManager.instance.eventHandler.InvokeEvent(@event);
        }
        catch (ApplicationException err)
        {
            Debug.LogError(err.Message);
        }

        yield return new WaitWhile(() => @event.IsPlaying);
    }

    public override string GetName()
    {
        return "Запустить событие";
    }
}