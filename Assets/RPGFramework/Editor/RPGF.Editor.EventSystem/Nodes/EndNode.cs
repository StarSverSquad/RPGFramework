using RPGF.Editor.EventSystem;
using RPGF.Editor.EventSystem.Attributes;
using RPGF.EventSystem.Default;
using UnityEngine;

[UseActionNodeAttribute]
public class EndNode : ActionNodeBase<EndAction>
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
