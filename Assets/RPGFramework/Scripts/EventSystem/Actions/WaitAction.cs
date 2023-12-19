using System.Collections;
using UnityEngine;

public class WaitAction : GraphActionBase
{
    public float time;

    public WaitAction() : base("Wait")
    {
        time = 1;
    }

    public override IEnumerator ActionCoroutine()
    {
        yield return new WaitForSeconds(time);
    }

    public override string GetHeader()
    {
        return "Ждать";
    }
}