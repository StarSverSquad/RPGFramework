
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGF.RPG
{
    public class RPGUsable : RPGBase
    {
        [Header("Настройки использования:")]
        [Space]
        public Sprite Icon;
        [Space]
        public Usability Usage;
        public UsabilityDirection Direction;
        [Space]
        public GraphEvent Event;
        [Space]
        public BattleAttackEffect VisualEffect;
        [Space]
        public bool WakeupCharacter = false;
        [Space]
        public bool ForAlive = true;
        public bool ForDeath = true;
        [Space]
        [HideInInspector]
        [SerializeReference]
        public List<EffectBase> Effects = new List<EffectBase>();

        public void InvokeEvent(Action OnEnd = null)
        {
            if (Event != null)
            {
                switch (Usage)
                {
                    case Usability.Any:
                        if (BattleManager.IsBattle)
                            Event.Invoke(BattleManager.Instance);
                        else if (!ExplorerManager.Instance.EventHandler.EventRuning)
                        {
                            Event.Invoke(ExplorerManager.Instance.EventHandler);
                            ExplorerManager.Instance.EventHandler.HandleEvent(Event);
                        }
                        break;
                    case Usability.Battle:
                        if (BattleManager.IsBattle)
                            Event.Invoke(BattleManager.Instance);
                        break;
                    case Usability.Explorer:
                        if (!BattleManager.IsBattle && !ExplorerManager.Instance.EventHandler.EventRuning)
                        {
                            Event.Invoke(ExplorerManager.Instance.EventHandler);
                            ExplorerManager.Instance.EventHandler.HandleEvent(Event);
                        }
                        break;
                }
            }

            OnEnd?.Invoke();
        }
    }
}