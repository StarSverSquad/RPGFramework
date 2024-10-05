using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplorerItemConsumeManager : MonoBehaviour
{
    private Coroutine consumeCoroutine;
    public bool IsCosuming => consumeCoroutine != null;

    public void CosumeItem(RPGConsumed item, RPGEntity who, RPGEntity target)
    {
        if (IsCosuming)
        {
            Debug.LogWarning("Some item is consuming now!");

            return;
        }

        consumeCoroutine = StartCoroutine(ConsumeCoroutine(item, who, target));
    }

    private IEnumerator ConsumeCoroutine(RPGConsumed item, RPGEntity who, RPGEntity target)
    {
        if (item.Usage == RPGCollectable.Usability.Battle || item.Usage == RPGCollectable.Usability.Noway
            || !GameManager.Instance.Inventory.HasItemSlot(item))
        {
            consumeCoroutine = null;

            yield break;
        }

        foreach (EffectBase effect in item.Effects)
        {
            yield return StartCoroutine(effect.Invoke(new BattleEntityInfo(who), new BattleEntityInfo(target)));
        }

        who.Heal = who.Heal == 0 ? 1 : who.Heal;

        GameManager.Instance.Inventory.AddToItemCount(item, -1);

        consumeCoroutine = null;
    }
}