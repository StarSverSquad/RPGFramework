using RPGF.RPG;
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
                RPGEnemy enemy = BattleManager.Instance.data.Enemys.First(i => i.Tag == EnemyTag);

                if (enemy == null)
                    throw new ApplicationException("Enemy not found!");

                enemy.DefaultDamage = newDamage;
                enemy.DefaultDefence = newDefance;
                enemy.DefaultAgility = newAgility;
                enemy.DefaultLuck = newLuck;

                enemy.UpdateStats();
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
        return "Изменить статы врага";
    }
}