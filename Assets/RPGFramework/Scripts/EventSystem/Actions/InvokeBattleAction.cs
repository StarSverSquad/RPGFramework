using RPGF.RPG;
using System.Collections;
using UnityEngine;

public class InvokeBattleAction : GraphActionBase
{
    public RPGBattleInfo battle;

    public bool fleePort;

    public override IEnumerator ActionCoroutine()
    {
        BattleManager.StartBattle(battle);

        yield return new WaitWhile(() => BattleManager.IsBattle);

        if (fleePort && BattleManager.Instance.Pipeline.IsFlee)
        {
            nextIndex = 1;
        }
        else if (BattleManager.Instance.Pipeline.IsLose)
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