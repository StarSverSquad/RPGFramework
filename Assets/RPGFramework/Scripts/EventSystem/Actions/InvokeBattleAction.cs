using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class InvokeBattleAction : GraphActionBase
{
    public RPGBattleInfo battle;

    public bool fleePort;

    public override IEnumerator ActionCoroutine()
    {
        BattleManager.StartBattle(battle);

        yield return new WaitWhile(() => BattleManager.IsBattle);

        if (fleePort && BattleManager.Instance.pipeline.IsFlee)
        {
            nextIndex = 1;
        }
        else if (BattleManager.Instance.pipeline.IsLose)
        {
            nextIndex = fleePort ? 2 : 1;
        }
        else
        {
            nextIndex = 0;
        }
    }

    public override string GetHeader()
    {
        return "Битва";
    }
}