using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorerEvent : MonoBehaviour
{
    public enum InteractionType
    {
        None, OnStep, OnSceneStart, EveryUpdate, EveryFixedUpdate, OnClick
    }

    public InteractionType Interaction;

    [Tooltip("Динамическое изменение Z координаты под -Y координату")]
    public bool DynamicOrderChange = false;

    [SerializeField]
    private bool hasExecuted = false;
    public bool HasExecuted => hasExecuted;

    public bool OnlyOne = true;
    public bool CacheResult = false;

    public bool Parallel = false;

    public GraphEvent Event;

    public string GUID { get; set; } = string.Empty;

    private void Start()
    {
        if (!DynamicOrderChange)
            transform.position = new Vector3(transform.position.x, transform.position.y, -transform.position.y);

        if (CacheResult)
            hasExecuted = GameManager.Instance.gameData.CachedObjectedEvents.Contains(GUID);

        if (Interaction == InteractionType.OnSceneStart)
            InvokeEvent();


        Event.OnEnd += Event_OnEnd;
    }

    public void InvokeEvent()
    {
        if (!hasExecuted && !Event.IsPlaying
            && (!ExplorerManager.instance.eventHandler.EventRuning || Parallel))
        {
            if (!Parallel)
                ExplorerManager.instance.eventHandler.HandleEvent(Event);

            Event.Invoke(ExplorerManager.instance);
        }
    }

    private void Event_OnEnd()
    {
        if (OnlyOne)
        {
            hasExecuted = true;

            if (CacheResult && !GameManager.Instance.gameData.CachedObjectedEvents.Contains(GUID))
                GameManager.Instance.gameData.CachedObjectedEvents.Add(GUID);
        }
    }

    private void FixedUpdate()
    {
        if (DynamicOrderChange)
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);

        if (Interaction == InteractionType.EveryFixedUpdate)
            InvokeEvent();
    }

    private void Update()
    {
        if (Interaction == InteractionType.EveryUpdate)
            InvokeEvent();
    }
}
