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

        private List<Port> inputs;
        private List<OutputPortWrapper> outputs;

        public EventGraphView view;

        public EventGraphNodeBase(ActionBase action)
        {
            this.action = action;

            inputs = new();
            outputs = new();

            extensionContainer.style.backgroundColor = (Color)new Color32(100, 100, 100, 200);
        }

        public override void SetPosition(Rect newPos)
        {
            MakeDirty();

            base.SetPosition(newPos);
        }

        protected Port CreateInputPort(string name)
        {
            Port port = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(EventGraphNodeBase));

            port.portName = name;
            port.portColor = new Color32(16, 130, 119, 255);

            inputContainer.Add(port);
            inputs.Add(port);

            return port;
        }
        protected Port CreateInputPort(string name, Color color)
        {
            Port port = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(EventGraphNodeBase));

            port.portName = name;
            port.portColor = color;

            inputContainer.Add(port);
            inputs.Add(port);

            return port;
        }

        protected Port CreateOutputPort(string name, string nextTag = ActionBase.DefaultNextTag)
        {
            Port port = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(EventGraphNodeBase));

            port.portName = name;
            port.portColor = new Color32(20, 104, 156, 255);

            outputs.Add(new OutputPortWrapper(port, nextTag));
            outputContainer.Add(port);

            return port;
        }
        protected Port CreateOutputPort(string name, Color color, string nextTag = ActionBase.DefaultNextTag)
        {
            Port port = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(EventGraphNodeBase));

            port.portName = name;
            port.portColor = color;

            outputs.Add(new OutputPortWrapper(port, nextTag));
            outputContainer.Add(port);

            return port;
        }

        public virtual void PortContructor()
        {
            CreateInputPort("Input");
            CreateOutputPort("Output");
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
            foreach (var port in outputs)
            {
                foreach (var edge in port.Port.connections)
                {
                    edge.input.Disconnect(edge);

                    edge.parent.Remove(edge);
                }

                port.Port.DisconnectAll();
            }


            foreach (var port in inputs)
            {
                foreach (var edge in port.connections)
                {
                    edge.output.Disconnect(edge);

                    edge.parent.Remove(edge);
                }

                port.DisconnectAll();
            }


            inputContainer.Clear();
            outputContainer.Clear();

            outputs = new();
            inputs = new();

            PortContructor();
        }

        public void ValidatePorts()
        {
            action.ClearNextActions();

            for (int i = 0; i < outputs.Count; i++)
            {
                var OutputPort = outputs[i];

                if (!OutputPort.Port.connections.Any())
                    continue;

                Port otherport;
                if (OutputPort.Port.connections.First().input != OutputPort.Port)
                {
                    otherport = OutputPort.Port.connections.First().input;
                }
                else if (OutputPort.Port.connections.First().output != OutputPort.Port)
                {
                    otherport = OutputPort.Port.connections.First().output;
                }
                else
                {
                    EditorUtility.DisplayDialog("Ошибка", "Ошибка привязки нодов и ивентов", "ок");
                    return;
                }

                var node = otherport.node as EventGraphNodeBase;
            }
        }
    }

    public class OutputPortWrapper
    {
        public Port Port { get; set; }

        public string NextTag { get; set; }

        public OutputPortWrapper(Port port, string nextTag = ActionBase.DefaultNextTag)
        {
            Port = port;
            NextTag = nextTag;
        }
    }
}