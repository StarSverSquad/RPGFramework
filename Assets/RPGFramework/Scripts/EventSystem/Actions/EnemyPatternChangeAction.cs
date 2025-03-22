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

    public BattleAttackPatternBase Pattern;

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
            
        var enemy = BattleManager.Data.Enemys.FirstOrDefault(enemy => enemy.Tag == EnemyTag);

        if (enemy == null)
        {
            Debug.LogError($"{Name}: враг не обнаружен!");
            yield break;
        }

        switch (Type)
        {
            case ChangeType.DeleteAll:
                enemy.Patterns.Clear();
                break;
            case ChangeType.Delete:
                var delPattern = enemy.Patterns.FindIndex(pat => pat.PatternTag == PatternTag);

                if (delPattern == -1)
                {
                    Debug.LogError($"{Name}: Паттерн не найден!");
                    yield break;
                }

                enemy.Patterns.RemoveAt(delPattern);
                break;
            case ChangeType.Add:
                enemy.Patterns.Add(Pattern);
                break;
        }
    }

    public override string GetHeader()
    {
        return "Изменение паттернов атаки врага";
    }
}
