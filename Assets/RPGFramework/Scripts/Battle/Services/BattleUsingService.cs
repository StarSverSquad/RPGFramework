using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleUsingService
{
    private BattleManager battleManager;
    private GameManager gameManager;

    public BattleUsingService(BattleManager battleManager, GameManager gameManager)
    {
        this.battleManager = battleManager;
        this.gameManager = gameManager;
    }

    public IEnumerator UseAbility(RPGAbility ability, BattleEntityInfo user, params BattleEntityInfo[] targets)
    {
        yield break;
    }

    public IEnumerator UseItem(RPGConsumed item, BattleEntityInfo user, params BattleEntityInfo[] targets)
    {
        if (targets.Any(i => i is BattleEnemyInfo) && item.VisualEffect != null)
        {
            VisualAttackEffect effect = battleManager.utility.SpawnAttackEffect(item.VisualEffect);

            effect.Invoke();

            yield return new WaitWhile(() => effect.IsAnimating);

            yield return new WaitForSeconds(0.5f);

            Object.Destroy(effect.gameObject);
        }

        foreach (BattleEntityInfo target in targets)
        {
            if (target is BattleEnemyInfo enemy)
            {

            }
            else if (target is BattleCharacterInfo character)
            {
                //foreach (var effect in item.Effects)
                //    yield return battleManager.pipeline.StartCoroutine(EffectInvoke(effect));


            }
        }

        gameManager.inventory.AddToItemCount(item, -1);
    }
}
