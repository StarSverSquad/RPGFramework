using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvokeEventAction : GraphActionBase
{
    public ExplorerEvent Event;

    public bool Wait;

    public InvokeEventAction() : base("InvokeEvent")
    {
        Event = null;

        Wait = false;
    }

    public override IEnumerator ActionCoroutine()
    {
        if (ExplorerManager.Instance.EventHandler.HandledEvent == gameEvent)
            ExplorerManager.Instance.EventHandler.ForceUnhandle();

        Event.InvokeEvent();

        if (Wait)
        {
            yield return new WaitWhile(() => Event.Event.IsPlaying);

            ExplorerManager.Instance.EventHandler.HandleEvent((GraphEvent)gameEvent);
        }

        yield break;
    }

    public override string GetHeader()
    {
        return "Запуск события";
    }
}
