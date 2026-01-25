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
    [GenerateActionNode("Изменение поведения врага", contextMenuPath: "Битва/Изменение поведения врага")]
    [Serializable]
    public class EnemyBehaviourChangeAction : ActionBase
    {
        public enum ChangeType
        {
            DeleteAll, Delete, Add
        }

        [Inject]
        private readonly BattleData _data;

        [ActionFieldOption("Тип изменения")]
        public ChangeType Type;

        [ActionFieldOption("Тег врага")]
        public string EnemyTag;
        [ActionFieldOption("Тег паттерна")]
        public string PatternTag;

        [ActionFieldOption("Паттерна", AllowSceneObjects = true)]
        public BattleEnemyBehaviourBase Pattern;

        public EnemyBehaviourChangeAction() : base()
        {
            EnemyTag = "";
            PatternTag = "";

            Pattern = null;

            Type = ChangeType.DeleteAll;
        }

        public override IEnumerator ActionCoroutine()
        {
            if (!BattleManager.IsBattle)
            {
                Debug.LogError($"{nameof(EnemyBehaviourChangeAction)}: запущен вне битвы!");
                yield break;
            }

            var enemy = _data.Enemys.FirstOrDefault(enemy => enemy.Tag == EnemyTag);

            if (enemy == null)
            {
                Debug.LogError($"{nameof(EnemyBehaviourChangeAction)}: враг не обнаружен!");
                yield break;
            }

            switch (Type)
            {
                case ChangeType.DeleteAll:
                    enemy.Behaviours.Clear();
                    break;
                case ChangeType.Delete:
                    var delPattern = enemy.Behaviours.FindIndex(pat => pat.BehaviourTag == PatternTag);

                    if (delPattern == -1)
                    {
                        Debug.LogError($"{nameof(EnemyBehaviourChangeAction)}: Поведение не найден!");
                        yield break;
                    }

                    enemy.Behaviours.RemoveAt(delPattern);
                    break;
                case ChangeType.Add:
                    enemy.Behaviours.Add(Pattern);
                    break;
            }
        }
    }
}