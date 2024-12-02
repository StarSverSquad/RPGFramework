using UnityEngine;

[UseActionNode]
public class StartNode : ActionNodeWrapper<StartAction>
{
    public StartNode(StartAction Action) : base(Action)
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
