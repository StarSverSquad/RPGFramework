using RPGF.RPG;
using System;
using System.Collections.Generic;

namespace RPGF.Core.Battle
{
    public class BattleData : RPGFrameworkBehaviour, IDisposable
    {
        public RPGBattleInfo BattleInfo { get; set; } = null;

        public List<RPGEnemy> Enemys { get; set; } = new();
        public List<BattleTurnData> TurnsData { get; set; } = new();

        public int Concentration { get; set; } = 0;

        public void Dispose()
        {
            Enemys.Clear();
            TurnsData.Clear();
            BattleInfo = null;

            Concentration = 0;
        }
    }
}