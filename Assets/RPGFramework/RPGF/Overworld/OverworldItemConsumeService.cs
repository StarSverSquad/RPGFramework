using RPGF.Core;
using RPGF.Domain.Interfaces;
using RPGF.RPG;
using System.Collections;
using UnityEngine;

namespace RPGF.Overworld
{
    public class OverworldItemConsumeService : IService
    {
        private Coroutine consumeCoroutine;
        public bool IsCosuming => consumeCoroutine != null;

        public void CosumeItem(MonoBehaviour listener, RPGConsumed item, RPGEntity who, RPGEntity target)
        {
            if (IsCosuming)
            {
                Debug.LogWarning("Some item is consuming now!");

                return;
            }

            consumeCoroutine = listener.StartCoroutine(ConsumeCoroutine(listener, item, who, target));
        }

        private IEnumerator ConsumeCoroutine(MonoBehaviour listener, RPGConsumed item, RPGEntity who, RPGEntity target)
        {
            if (item.Usage == Usability.Battle || item.Usage == Usability.Noway
                || !GlobalManager.Instance.Inventory.HasItemSlot(item))
            {
                consumeCoroutine = null;

                yield break;
            }

            foreach (var effect in item.Effects)
            {
                yield return listener.StartCoroutine(effect.Invoke(who, target));
            }

            who.Heal = who.Heal == 0 ? 1 : who.Heal;

            GlobalManager.Instance.Inventory.AddToItemCount(item, -1);

            consumeCoroutine = null;
        }
    }
}