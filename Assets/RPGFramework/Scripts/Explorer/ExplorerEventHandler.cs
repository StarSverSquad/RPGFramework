using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorerEventHandler : MonoBehaviour
{
    [SerializeField]
    private GraphEvent CurrentEvent;
    public GraphEvent HandledEvent => CurrentEvent;

    public event Action OnHandle;
    public event Action OnUnhandle;

    public bool EventRuning => CurrentEvent != null;

    public void InvokeEvent(GraphEvent e)
    {
        e.Invoke(this);

        HandleEvent(e);
    }

    public void HandleEvent(GraphEvent e)
    {
        if (!EventRuning)
        {
            CurrentEvent = e;

            e.OnEnd += E_OnEnd;

            OnHandle?.Invoke();
        }
    }

    public void ForceUnhandle()
    {
        if (EventRuning)
        {
            CurrentEvent.OnEnd -= E_OnEnd;

            CurrentEvent = null;

            OnUnhandle?.Invoke();
        }
    }

    private void E_OnEnd()
    {
        CurrentEvent.OnEnd -= E_OnEnd;

        CurrentEvent = null;

        OnUnhandle?.Invoke();
    }
}
