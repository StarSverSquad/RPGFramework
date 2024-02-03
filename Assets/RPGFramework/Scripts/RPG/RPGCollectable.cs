using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultItem", menuName = "RPG/DefaultItem")]
public class RPGCollectable : ScriptableObject
{
    public enum Rareness
    {
        Common, Rare, Special, Key
    }

    public enum Usability
    {
        Any, Noway, Battle, Explorer
    }

    public string Tag;
    public string Name;
    [Multiline(2)]
    public string Description;

    public Rareness Rare;

    public Usability Usage;

    public Sprite Icon;

    public GraphEvent Event;

    public void InvokeEvent(Action OnEnd = null)
    {
        if (Event != null)
        {
            switch (Usage)
            {
                case Usability.Any:
                    if (BattleManager.IsBattle)
                        Event.Invoke(BattleManager.Instance);
                    else if (!ExplorerManager.Instance.eventHandler.EventRuning)
                    {
                        Event.Invoke(ExplorerManager.Instance.eventHandler);
                        ExplorerManager.Instance.eventHandler.HandleEvent(Event);
                    }
                        break;
                case Usability.Battle:
                    if (BattleManager.IsBattle)
                        Event.Invoke(BattleManager.Instance);
                    break;
                case Usability.Explorer:
                    if (!BattleManager.IsBattle && !ExplorerManager.Instance.eventHandler.EventRuning)
                    {
                        Event.Invoke(ExplorerManager.Instance.eventHandler);
                        ExplorerManager.Instance.eventHandler.HandleEvent(Event);
                    }
                    break;
                case Usability.Noway:
                    break;
            }
        }

        OnEnd?.Invoke();
    }
}
