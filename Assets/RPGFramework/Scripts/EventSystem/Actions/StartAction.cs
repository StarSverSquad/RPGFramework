using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAction : GraphActionBase
{
    public StartAction() : base("Start")
    {
    }

    public override IEnumerator ActionCoroutine()
    {
        yield break;
    }

    public override string GetInfo()
    {
        return "����� �������";
    }

    public override string GetHeader()
    {
        return "�����";
    }
}
