using RPGF.Battle;
using RPGF.Battle.Enemy;
using RPGF.Domain.DI;
using RPGF.EventSystem;
using RPGF.EventSystem.Attributes;
using RPGF.RPG;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;


namespace RPGF.Actions.Condition
{
    [GenerateActionNode("Анимация врага", "Активирует триггер в выбранном аниматоре", "Битва/Анимация врага")]
    public class ChangeEnemyModelAnimationAction : ActionBase
    {
        [Inject]
        private readonly BattleEnemyModelsManager _models;
        [Inject]
        private readonly BattleData _data;

        [ActionFieldOption("Тег врага:")]
        public string EnemyTag;
        [ActionFieldOption("Тег аниматора:")]
        public string AnimatorTag;
        [ActionFieldOption("Триггер:")]
        public string Trigger;

        public ChangeEnemyModelAnimationAction() : base()
        {
            EnemyTag = string.Empty;
            AnimatorTag = string.Empty;
            Trigger = string.Empty;
        }

        public override IEnumerator ActionCoroutine()
        {
            try
            {
                if (BattleManager.IsBattle)
                {
                    RPGEnemy enemy = _data.Enemys.First(i => i.Tag == EnemyTag);

                    if (enemy == null)
                        throw new ApplicationException("Враг не найден!");

                    BattleEnemyModel model = _models.GetModel(enemy);

                    if (model == null)
                        throw new ApplicationException("Моделька врага не найдена!");

                    model.GetAnimator(AnimatorTag).SetTrigger(Trigger);
                }
            }
            catch (ApplicationException error)
            {
                Debug.LogException(error);
            }

            yield break;
        }
    }
}