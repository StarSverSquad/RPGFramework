using RPGF.Core.Battle;
using RPGF.Core.RPGEffect;
using RPGF.EventSystem;
using RPGF.Explorer;
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
        public GlobalEvent Event;
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
        public List<RPGEffectBase> Effects = new();
    }
}