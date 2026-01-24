using RPGF.Core.Battle;
using System.Collections;
using UnityEngine;

namespace RPGF.Battle
{
    public class BattleVisualTransmitionManager : MonoBehaviour
    {
        [SerializeField]
        private Canvas currentCanvas;

        [SerializeField]
        private BattleTransmitionBase defaultEffect;
        public BattleTransmitionBase DefaultEffect => defaultEffect;

        private BattleTransmitionBase effect;
        private GameObject effectObject;

        public BattleTransmitionBase CustomEffect => effect;

        public void InitializeEffect(BattleTransmitionBase effect)
        {
            if (effect == null)
                return;

            if (this.effect != null)
                DisposeEffect();

            effectObject = Instantiate(effect.gameObject, transform);

            this.effect = effectObject.GetComponent<BattleTransmitionBase>();
        }

        public void DisposeEffect()
        {
            if (effect == null)
                return;

            Destroy(effectObject);

            effectObject = null;
            effect = null;
        }

        public IEnumerator InvokePartOne()
        {
            if (effect != null)
                yield return effect.PartOne();
            else
                yield return defaultEffect.PartOne();
        }

        public IEnumerator InvokePartTwo()
        {
            if (effect != null)
                yield return effect.PartTwo();
            else
                yield return defaultEffect.PartTwo();
        }
    }
}