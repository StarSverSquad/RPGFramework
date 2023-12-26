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

    private VisualBattleTransmitionEffectBase customEffect;
    private GameObject customEffectObject;

    public VisualBattleTransmitionEffectBase CustomEffect => customEffect;

    public void InitializeCustomEffect(VisualBattleTransmitionEffectBase effect)
    {
        if (effect == null)
            return;

        if (customEffect != null)
            DisposeCustomEffect();

        customEffectObject = Instantiate(effect.gameObject, transform);

        customEffect = customEffectObject.GetComponent<VisualBattleTransmitionEffectBase>();
    }

    public void DisposeCustomEffect()
    {
        if (customEffect == null)
            return;

        Destroy(customEffectObject);

        customEffectObject = null;
        customEffect = null;
    }

    public IEnumerator InvokePartOne()
    {
        if (customEffect != null)
            return customEffect.PartOne();
        else
            return defaultEffect.PartOne();
    }

    public IEnumerator InvokePartTwo()
    {
        if (customEffect != null)
            return customEffect.PartTwo();
        else
            return defaultEffect.PartTwo();
    }
}