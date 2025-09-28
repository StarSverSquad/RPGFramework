using RPGF.EventSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace RPGF.Editor.EventSystem
{
    public abstract class EventGraphNodeBase : Node
    {
        public string GUID;

        public ActionBase action;

        public EventGraphView view;

        public List<PortWrapper> Inputs { get; private set; }
        public List<OutputPortWrapper> Outputs { get; private set; }

        public EventGraphNodeBase(ActionBase action)
        {
            this.action = action;

            Inputs = new();
            Outputs = new();

            extensionContainer.style.backgroundColor = (Color)new Color32(100, 100, 100, 200);
        }

        public override void SetPosition(Rect newPos)
        {
            MakeDirty();

            base.SetPosition(newPos);
        }

        protected Port CreateInputPort(string name, string tag)
        {
            Port port = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(EventGraphNodeBase));

            port.portName = name;
            port.portColor = new Color32(16, 130, 119, 255);

            inputContainer.Add(port);
            Inputs.Add(new PortWrapper(tag, port));

            return port;
        }
        protected Port CreateInputPort(string name, string tag, Color color)
        {
            Port port = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(EventGraphNodeBase));

            port.portName = name;
            port.portColor = color;

            inputContainer.Add(port);
            Inputs.Add(new PortWrapper(tag, port));

            return port;
        }

        protected Port CreateOutputPort(string name, string tag, string nextTag = ActionBase.DefaultNextTag)
        {
            Port port = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(EventGraphNodeBase));

            port.portName = name;
            port.portColor = new Color32(20, 104, 156, 255);

            Outputs.Add(new OutputPortWrapper(tag, port, nextTag));
            outputContainer.Add(port);

            return port;
        }
        protected Port CreateOutputPort(string name, string tag, Color color, string nextTag = ActionBase.DefaultNextTag)
        {
            Port port = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(EventGraphNodeBase));

            port.portName = name;
            port.portColor = color;

            Outputs.Add(new OutputPortWrapper(tag, port, nextTag));
            outputContainer.Add(port);

            return port;
        }

        public virtual void PortContructor()
        {
            CreateInputPort("Вход", "Input");

            foreach (var next in action.Nexts)
            {
                CreateOutputPort(next.Name, next.Tag, next.Tag);
            }
        }

        public abstract void UIContructor();

        public void MakeDirty()
        {
            view.MakeDirty();
        }

        public void UpdateUI()
        {
            extensionContainer.Clear();
            UIContructor();
        }

        public void UpdatePorts()
        {
            foreach (var port in Outputs)
            {
                foreach (var edge in port.InnerPort.connections)
                {
                    edge.input.Disconnect(edge);

                    edge.parent.Remove(edge);
                }

                port.InnerPort.DisconnectAll();
            }


            foreach (var port in Inputs)
            {
                foreach (var edge in port.InnerPort.connections)
                {
                    edge.output.Disconnect(edge);

                    edge.parent.Remove(edge);
                }

                port.InnerPort.DisconnectAll();
            }


            inputContainer.Clear();
            outputContainer.Clear();

            Outputs = new();
            Inputs = new();

            PortContructor();
        }

        public void ValidatePorts()
        {
            action.ClearNextActions();

            for (int i = 0; i < Outputs.Count; i++)
            {
                var OutputPort = Outputs[i];

                if (!OutputPort.InnerPort.connections.Any())
                    continue;

                Port otherport;
                if (OutputPort.InnerPort.connections.First().input != OutputPort.InnerPort)
                {
                    otherport = OutputPort.InnerPort.connections.First().input;
                }
                else if (OutputPort.InnerPort.connections.First().output != OutputPort.InnerPort)
                {
                    otherport = OutputPort.InnerPort.connections.First().output;
                }
                else
                {
                    EditorUtility.DisplayDialog("Ошибка", "Ошибка привязки нодов и ивентов", "ок");
                    return;
                }

                var node = otherport.node as EventGraphNodeBase;

                action.SetNextAction(node.action, OutputPort.NextTag);
            }
        }
    }
}