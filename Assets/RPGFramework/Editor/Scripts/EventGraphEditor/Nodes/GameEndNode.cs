using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[UseActionNode]
public class GameEndNode : ActionNodeWrapper<GameEndAction>
{
    public GameEndNode(GameEndAction Action) : base(Action)
    {
    }

    public override void PortContructor()
    {
        CreateInputPort("Input", new Color(110f / 255f, 13f / 255f, 13f / 255f));
    }

    public override void UIContructor() { }
}
