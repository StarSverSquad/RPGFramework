using System;
using System.Collections;
using UnityEngine;

public class BattleTest : CustomActionBase
{
    public RPGBattleInfo Battle;

    protected override IEnumerator ActionCoroutine()
    {
        BattleManager.StartBattle(Battle);

        yield return new WaitWhile(() => BattleManager.IsBattle);
    }
}