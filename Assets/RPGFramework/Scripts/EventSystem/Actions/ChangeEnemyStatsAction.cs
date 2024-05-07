using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class ChangeEnemyStatsAction : GraphActionBase
{
    public string EnemyTag;

    public int newDamage;
    public int newDefance;
    public int newLuck;
    public int newAgility;

    public ChangeEnemyStatsAction() : base("ChangeEnemyStats")
    {
        newDamage = 0;
        newDefance = 0;
        newLuck = 0;
        newAgility = 0;
    }

    public override IEnumerator ActionCoroutine()
    {
        try
        {
            if (BattleManager.IsBattle)
            {
                BattleEnemyInfo enemy = BattleManager.Instance.data.Enemys.First(i => i.Entity.Tag == EnemyTag);

                if (enemy == null)
                    throw new ApplicationException("Enemy not found!");

                enemy.Entity.DefaultDamage = newDamage;
                enemy.Entity.DefaultDefence = newDefance;
                enemy.Entity.DefaultAgility = newAgility;
                enemy.Entity.DefaultLuck = newLuck;

                enemy.Entity.UpdateStats();
            }
        }
        catch (ApplicationException error)
        {
            Debug.LogException(error);
        }

        yield break;
    }

    public override string GetHeader()
    {
        return "Сменить анимацию врага";
    }
}