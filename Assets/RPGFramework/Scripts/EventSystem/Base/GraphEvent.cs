using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InnerEvent", menuName = "RPGFramework/InnerEvent")]
public class GraphEvent : GameEventBase
{
    public GraphEventMeta Meta = new GraphEventMeta();

    public event Action GraphChanged;

    public event Action OnStart;
    public event Action OnEnd;

    public void DispathGraphChanges() => GraphChanged?.Invoke();

    protected override IEnumerator EventCoroutine()
    {
        OnStart?.Invoke();

        GraphActionBase current = null;

        foreach (GameActionBase action in Actions)
        {
            if (action is StartAction)
            {
                current = (StartAction)action;
                break;
            }
        }

        if (current == null && Actions.Count > 0)
        {
            Debug.LogWarning("Не найдено стартовое событие!");
            current = (GraphActionBase)Actions[0];
        }

        while (current != null)
        {
            yield return current.Launch(this);

            current = current.Next;
        }

        EndEventPart();

        OnEnd?.Invoke();
    }
}
