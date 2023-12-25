using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class VisualBattleTransmitionDefault : VisualBattleTransmitionEffectBase
{
    public AnimationCurve InCurve;
    public AnimationCurve OutCurve;

    public Image BlackScreen;

    public override IEnumerator PartOne()
    {
        float time = 0;

        while (time < InCurve.keys.Last().time)
        {
            BlackScreen.color = new Color(0f, 0f, 0f, InCurve.Evaluate(time));

            yield return new WaitForFixedUpdate();

            time += Time.fixedDeltaTime;
        }

        BlackScreen.color = new Color(0f, 0f, 0f, 1f);
    }

    public override IEnumerator PartTwo()
    {
        float time = 0;

        while (time < OutCurve.keys.Last().time)
        {
            BlackScreen.color = new Color(0f, 0f, 0f, OutCurve.Evaluate(time));

            yield return new WaitForFixedUpdate();

            time += Time.fixedDeltaTime;
        }

        BlackScreen.color = new Color(0f, 0f, 0f, 0f);
    }
}