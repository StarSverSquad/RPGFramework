using System;
using System.Collections;
using UnityEngine;

public class BattleTest : CustomActionBase
{
    public RPGBattleInfo Battle;
    public RPGCollectable testItem;
    public RPGCollectable testItem1;
    public RPGCollectable testItem2;

    protected override IEnumerator ActionCoroutine()
    {
        GameManager.Instance.inventory.AddToItemCount(testItem, 99);
        GameManager.Instance.inventory.AddToItemCount(testItem1, 99);
        GameManager.Instance.inventory.AddToItemCount(testItem2, 99);

        BattleManager.StartBattle(Battle);

        yield return new WaitWhile(() => BattleManager.IsBattle);
    }
}