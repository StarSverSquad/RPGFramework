using RPGF.Core;
using RPGF.EventSystem.Graph;
using System;
using UnityEngine;

namespace RPGF.Explorer
{
    public class ExplorerEventHandler : RPGFrameworkBehaviour
    {
        [SerializeField]
        private GraphEvent CurrentEvent;
        public GraphEvent HandledEvent => CurrentEvent;

        public event Action OnHandle;
        public event Action OnUnhandle;

        public bool EventPlaying => CurrentEvent != null && CurrentEvent.IsPlaying;
        public bool EventExists => CurrentEvent != null;

        public void InvokeEvent(GraphEvent e)
        {
            e.Invoke(this, Local.DI);

            HandleEvent(e);
        }

        public void HandleEvent(GraphEvent e)
        {
            if (!EventExists)
            {
                CurrentEvent = e;

                e.OnEnd += E_OnEnd;

                OnHandle?.Invoke();
            }
        }

        public void ForceUnhandle()
        {
            if (EventExists)
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

}