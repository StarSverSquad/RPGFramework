using UnityEngine;

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
            && (!Explorer.EventHandler.EventRuning || Parallel))
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
        return Game.GameData.BlockedLocationEvents.Contains(EventTag);
    }

    public void BlockEvent()
    {
        if (!Game.GameData.BlockedLocationEvents.Contains(EventTag))
            Game.GameData.BlockedLocationEvents.Add(EventTag);
    }

    private void OnDestroy()
    {
        InnerEvent.OnEnd -= Event_OnEnd;
    }
}
