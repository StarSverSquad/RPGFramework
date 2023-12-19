using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndNode : ActionNodeBase
{
    public EndNode(EndAction action) : base(action)
    {
    }

    public override void PortContructor()
    {
        CreateInputPort("Input", new Color(110f / 255f, 13f / 255f, 13f / 255f));
    }

    public override void UIContructor()
    {
        
    }
}
