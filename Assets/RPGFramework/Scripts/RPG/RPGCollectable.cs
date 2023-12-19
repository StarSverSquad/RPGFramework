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

    public string Name;

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
                        Event.Invoke(BattleManager.instance);
                    else if (!ExplorerManager.instance.eventHandler.EventRuning)
                    {
                        Event.Invoke(ExplorerManager.instance.eventHandler);
                        ExplorerManager.instance.eventHandler.HandleEvent(Event);
                    }
                        break;
                case Usability.Battle:
                    if (BattleManager.IsBattle)
                        Event.Invoke(BattleManager.instance);
                    break;
                case Usability.Explorer:
                    if (!BattleManager.IsBattle && !ExplorerManager.instance.eventHandler.EventRuning)
                    {
                        Event.Invoke(ExplorerManager.instance.eventHandler);
                        ExplorerManager.instance.eventHandler.HandleEvent(Event);
                    }
                    break;
                case Usability.Noway:
                    break;
            }
        }

        OnEnd?.Invoke();
    }
}
