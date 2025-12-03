using RPGF.Core;
using RPGF.Domain.DI;
using RPGF.EventSystem.Graph;
using RPGF.Explorer;
using UnityEngine;

namespace RPGF.EventSystem
{
    public class LocationEvent : RPGFrameworkBehaviour
    {
        public enum InteractionType
        {
            None, OnStep, OnSceneStart, EveryUpdate, EveryFixedUpdate, OnClick
        }

        public InteractionType Interaction;

        public bool OnlyOne = true;
        public bool Parallel = false;

        [SerializeField]
        [SerializeReference]
        public GraphEvent InnerEvent;

        public string EventTag => $"{name}_event";

        private void Start()
        {
            InnerEvent.OnEnd += Event_OnEnd;

            if (Interaction == InteractionType.OnSceneStart && !IsBlocked())
                InvokeEvent();
        }

        private void FixedUpdate()
        {
            if (Interaction == InteractionType.EveryFixedUpdate && !IsBlocked())
                InvokeEvent();
        }

        private void Update()
        {
            if (Interaction == InteractionType.EveryUpdate && !IsBlocked())
                InvokeEvent();
        }

        public void InvokeEvent()
        {
            if (!IsBlocked() && !InnerEvent.IsPlaying
                && (!Explorer.EventHandler.EventPlaying || Parallel))
            {
                if (!Parallel)
                    Explorer.EventHandler.HandleEvent(InnerEvent);

                InnerEvent.Invoke(Explorer, Local.DI);
            }
        }

        private void Event_OnEnd()
        {
            if (OnlyOne)
                BlockEvent();
        }

        public bool IsBlocked()
        {
            return Global.GameData.BlockedLocationEvents.Contains(EventTag);
        }

        public void BlockEvent()
        {
            if (!Global.GameData.BlockedLocationEvents.Contains(EventTag))
                Global.GameData.BlockedLocationEvents.Add(EventTag);
        }

        private void OnDestroy()
        {
            InnerEvent.OnEnd -= Event_OnEnd;
        }
    }
}