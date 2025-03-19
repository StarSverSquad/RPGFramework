using System;
using System.Collections;
using UnityEngine;

public class BattleVisualTransmitionManager : MonoBehaviour
{
    [SerializeField]
    private Canvas currentCanvas;

    [SerializeField]
    private VisualBattleTransmitionEffectBase defaultEffect;
    public VisualBattleTransmitionEffectBase DefaultEffect => defaultEffect;

    private VisualBattleTransmitionEffectBase effect;
    private GameObject effectObject;

    public VisualBattleTransmitionEffectBase CustomEffect => effect;

    public void InitializeEffect(VisualBattleTransmitionEffectBase effect)
    {
        if (effect == null)
            return;

        if (this.effect != null)
            DisposeEffect();

        effectObject = Instantiate(effect.gameObject, transform);

        this.effect = effectObject.GetComponent<VisualBattleTransmitionEffectBase>();
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