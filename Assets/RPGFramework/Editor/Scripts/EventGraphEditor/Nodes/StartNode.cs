using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNode : ActionNodeBase
{
    public StartNode(StartAction action) : base(action)
    {
    }

    public override void PortContructor()
    {
        CreateOutputPort("Output", new Color(0.06f, 0.43f, 0.05f));
    }

    public override void UIContructor()
    {

    }
}
