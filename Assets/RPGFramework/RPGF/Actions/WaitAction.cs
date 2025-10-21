using RPGF.EventSystem;
using System.Collections;
using UnityEngine;

public class WaitAction : ActionBase
{
    public float time;

    public WaitAction() : base("IsWait")
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

    public override object Clone()
    {
        return new WaitAction()
        {
            time = time
        };
    }
}