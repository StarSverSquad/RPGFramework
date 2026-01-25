using RPGF.Core.Battle;
using RPGF.Battle;
using RPGF.Domain.DI;
using RPGF.EventSystem;
using RPGF.EventSystem.Attributes;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace RPGF.Actions
{
    [GenerateActionNode("Изменить статы врага", contextMenuPath: "Битва/Изменить статы врага")]
    [Serializable]
    public class ChangeEnemyStatsAction : ActionBase
    {
        [Inject]
        private readonly BattleData _battleData;

        [ActionFieldOption("Тег врага:")]
        public string EnemyTag;

        [ActionFieldOption("Урон:")]
        public int newDamage;
        [ActionFieldOption("Защита:")]
        public int newDefance;
        [ActionFieldOption("Удача:")]
        public int newLuck;
        [ActionFieldOption("Ловкость:")]
        public int newAgility;

        public ChangeEnemyStatsAction() : base()
        {
            newDamage = 0;
            newDefance = 0;
            newLuck = 0;
            newAgility = 0;
        }

        public override IEnumerator ActionCoroutine()
        {
            if (BattleManager.IsBattle)
            {
                var enemy = _battleData.Enemys.First(i => i.Tag == EnemyTag);

                if (enemy != null)
                {
                    Debug.LogError("Враг не найден!");

                    yield break;
                }

                enemy.DefaultDamage = newDamage;
                enemy.DefaultDefence = newDefance;
                enemy.DefaultAgility = newAgility;
                enemy.DefaultLuck = newLuck;

                enemy.UpdateStats();
            }

            yield break;
        }
    }
}