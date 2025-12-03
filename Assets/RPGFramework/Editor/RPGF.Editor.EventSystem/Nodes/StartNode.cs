using RPGF.Editor.EventSystem.Attributes;
using RPGF.EventSystem.Default;
using UnityEngine;

namespace RPGF.Editor.EventSystem.Nodes
{
    [UseActionNode("Старт", Ignore = true)]
    public class StartNode : ActionNodeBase<StartAction>
    {
        public StartNode(StartAction Action) : base(Action)
        {
        }

        public override void PortContructor()
        {
            CreateOutputPort("->", "Output", new Color(0.06f, 0.43f, 0.05f));
        }
    }
}