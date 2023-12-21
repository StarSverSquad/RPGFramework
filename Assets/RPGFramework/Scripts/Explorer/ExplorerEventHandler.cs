using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorerEventHandler : MonoBehaviour
{
    [SerializeField]
    private GraphEvent CurrentEvent;

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
        }
    }

    private void E_OnEnd()
    {
        CurrentEvent.OnEnd -= E_OnEnd;

        CurrentEvent = null;
    }
}
