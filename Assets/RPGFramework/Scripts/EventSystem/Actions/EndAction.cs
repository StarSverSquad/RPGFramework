using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAction : GraphActionBase
{
    public EndAction() : base("End")
    {
    }

    public override IEnumerator ActionCoroutine()
    {
        yield break;
    }

    public override string GetHeader()
    {
        return "�����";
    }

    public override string GetInfo()
    {
        return "����� �������";
    }
}
