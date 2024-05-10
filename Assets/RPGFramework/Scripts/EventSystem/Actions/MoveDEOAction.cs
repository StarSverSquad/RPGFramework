using System;
using System.Collections;
using UnityEngine;

[Obsolete]
public class MoveDEOAction : GraphActionBase
{
    public DynamicExplorerObject Model;

    public Vector2 Offset;
    public float Speed;

    public bool IsWait;

    public MoveDEOAction() : base("MoveDEO")
    {
        Model = null;
        Offset = Vector2.zero;
        IsWait = true;
        Speed = 1f;
    }

    public override IEnumerator ActionCoroutine()
    {
        Model.TranslateBySpeed(Offset, Speed);

        if (IsWait)
            yield return new WaitWhile(() => Model.IsMove);
    }

    public override string GetHeader()
    {
        return "Двигать персонажа на сцене";
    }
}