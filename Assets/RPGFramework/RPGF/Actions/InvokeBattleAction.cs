using RPGF.Battle;
using RPGF.Domain.DI;
using RPGF.EventSystem;
using RPGF.RPG;
using System;
using System.Collections;
using UnityEngine;

namespace RPGF.Actions
{
    [Serializable]
    public class InvokeBattleAction : ActionBase
    {
        public const string WinNextTag = "Win";
        public const string FleeNextTag = "Flee";
        public const string LoseNextTag = "Lose";

        [Inject]
        private readonly BattlePipeline _pipeline = null!;
        [Inject]
        private readonly BattleUtility _utility = null!;

        public RPGBattleInfo battle;

        public bool branchFlee;

        public InvokeBattleAction() : base()
        {
            Nexts.Clear();

            AddNext(WinNextTag, "При победе");
            AddNext(FleeNextTag, "При побеге");
            AddNext(LoseNextTag, "При поражении");
        }

        public override IEnumerator ActionCoroutine()
        {
            _utility.StartBattle(battle);

            yield return new WaitWhile(() => BattleManager.IsBattle);

            if (battle.CanFlee && _pipeline.IsFlee && branchFlee)
            {
                SetNext(FleeNextTag);
            }
            else if (battle.CanLose && _pipeline.IsLose)
            {
                SetNext(LoseNextTag);
            }
            else
            {
                SetNext(WinNextTag);
            }
        }
    }
}
