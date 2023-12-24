using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BattleEnemyInfo : BattleEntityInfo
{
    public RPGEnemy Enemy => Entity as RPGEnemy;

    public BattleEnemyInfo(RPGEnemy entity) : base(UnityEngine.Object.Instantiate(entity))
    {
        Enemy.InitializeEntity();
    }
}