using RPGF.Core;
using RPGF.EventSystem.Graph;
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

        [SerializeReference]
        public GraphEvent InnerEvent;

        public string EventTag => $"{name}_event";

        private void Start()
        {
            if (Interaction == InteractionType.OnSceneStart && !IsBlocked())
                InvokeEvent();


            InnerEvent.OnEnd += Event_OnEnd;
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

                InnerEvent.Invoke(Explorer);
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