using UnityEngine;

public class EndNode : ActionNodeWrapper<EndAction>
{
    public EndNode(EndAction Action) : base(Action)
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
