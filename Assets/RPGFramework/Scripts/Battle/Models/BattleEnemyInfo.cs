using UnityEngine;

public class BattleEnemyInfo : BattleEntityInfo
{
    public RPGEnemy Enemy => Entity as RPGEnemy;

    public BattleEnemyInfo(RPGEnemy entity) : base(Object.Instantiate(entity))
    {
        Enemy.InitializeEntity();
    }
}