using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAction : GraphActionBase
{
    public float Chance;

    public RandomAction() : base("Random")
    {
        Chance = 0.5f;
    }

    public override IEnumerator ActionCoroutine()
    {
        float result = Random.Range(0f, 1f);

        nextIndex = result <= Chance ? 0 : 1;

        yield break;
    }

    public override string GetHeader()
    {
        return "Случайное действие";
    }
}
