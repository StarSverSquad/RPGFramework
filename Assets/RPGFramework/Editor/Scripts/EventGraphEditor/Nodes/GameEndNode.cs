using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndNode : ActionNodeBase
{
    public GameEndNode(GameEndAction action) : base(action)
    {
    }

    public override void PortContructor()
    {
        CreateInputPort("Input", new Color(110f / 255f, 13f / 255f, 13f / 255f));
    }

    public override void UIContructor() { }
}
