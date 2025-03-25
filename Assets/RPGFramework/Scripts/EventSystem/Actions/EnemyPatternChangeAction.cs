using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyPatternChangeAction : GraphActionBase
{
    public enum ChangeType
    {
        DeleteAll, Delete, Add
    }

    public string EnemyTag;
    public string PatternTag;

    public BattleEnemyBehaviourBase Pattern;

    public ChangeType Type;

    public EnemyPatternChangeAction() : base("EnemyPatternChange")
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
            Debug.LogError($"{Name}: запущен вне битвы!");
            yield break;
        }
            
        var enemy = BattleManager._Data.Enemys.FirstOrDefault(enemy => enemy.Tag == EnemyTag);

        if (enemy == null)
        {
            Debug.LogError($"{Name}: враг не обнаружен!");
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
                    Debug.LogError($"{Name}: Паттерн не найден!");
                    yield break;
                }

                enemy.Behaviours.RemoveAt(delPattern);
                break;
            case ChangeType.Add:
                enemy.Behaviours.Add(Pattern);
                break;
        }
    }

    public override string GetHeader()
    {
        return "Изменение паттернов атаки врага";
    }
}
