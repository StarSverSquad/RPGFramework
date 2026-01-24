using RPGF.EventSystem;
using UnityEditor.Experimental.GraphView;

namespace RPGF.Editor.EventSystem
{
    public class PortWrapper
    {
        public Port InnerPort { get; set; }

        public string Tag
        {
            get => (string)InnerPort.userData;
            set => InnerPort.userData = value;
        }

        public PortWrapper(string tag, Port port)
        {
            InnerPort = port;
            Tag = tag;
        }
    }

    public class OutputPortWrapper : PortWrapper
    {
        public string NextTag { get; set; }

        public OutputPortWrapper(string tag, Port port, string nextTag = ActionBase.DefaultNextTag) : base(tag, port)
        {
            NextTag = nextTag;
        }
    }
}
