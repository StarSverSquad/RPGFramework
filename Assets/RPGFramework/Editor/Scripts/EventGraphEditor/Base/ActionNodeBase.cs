using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// Начальное название ноды должно полностью сответсвовать названию действия ActAction = ActNode
/// </summary>
public abstract class ActionNodeBase : Node
{
    public string GUID;

    public GraphActionBase action;

    private List<Port> inputs;
    private List<Port> outputs;

    public EventGraphView view;

    public ActionNodeBase(GraphActionBase action)
    {
        this.action = action;

        inputs = new List<Port>();
        outputs = new List<Port>();

        tooltip = action.GetInfo();
        title = action.GetHeader();

        extensionContainer.style.backgroundColor = (Color)(new Color32(100, 100, 100, 200));
    }

    public override void SetPosition(Rect newPos)
    {
        MakeDirty();

        base.SetPosition(newPos);
    }

    protected Port CreateInputPort(string name)
    {
        Port port = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(ActionNodeBase));

        port.portName = name;
        port.portColor = new Color32(16, 130, 119, 255);

        inputContainer.Add(port);
        inputs.Add(port);

        return port;
    }
    protected Port CreateInputPort(string name, Color color)
    {
        Port port = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(ActionNodeBase));

        port.portName = name;
        port.portColor = color;

        inputContainer.Add(port);
        inputs.Add(port);

        return port;
    }

    protected Port CreateOutputPort(string name)
    {
        Port port = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(ActionNodeBase));

        port.portName = name;
        port.portColor = new Color32(20, 104, 156, 255); 

        outputs.Add(port);
        outputContainer.Add(port);

        return port;
    }
    protected Port CreateOutputPort(string name, Color color)
    {
        Port port = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(ActionNodeBase));

        port.portName = name;
        port.portColor = color;

        outputs.Add(port);
        outputContainer.Add(port);

        return port;
    }

    public virtual void PortContructor()
    {
        CreateInputPort("Input");
        CreateOutputPort("Output");
    }

    public abstract void UIContructor();

    /// <summary>
    /// Помечает объект изменённым
    /// </summary>
    public void MakeDirty()
    {
        view.MakeDirty();
    }

    /// <summary>
    /// Обновляет содержимое ноды
    /// </summary>
    public void UpdateUI()
    {
        extensionContainer.Clear();
        UIContructor();
    }

    public void UpdatePorts()
    {
        foreach (var port in outputs)
        {
            foreach (var edge in port.connections)
            {
                edge.input.Disconnect(edge);

                edge.parent.Remove(edge);
            }

            port.DisconnectAll();
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

        outputs = new List<Port>();
        inputs = new List<Port>();

        PortContructor();
    }

    public void ApplyPorts()
    {
        action.NextActions.Clear();

        for (int i = 0; i < outputs.Count; i++)
        {
            if (outputs[i].connections.ToList().Count == 0)
                continue;

            Port otherport;
            if (outputs[i].connections.ToList()[0].input != outputs[i])
                otherport = outputs[i].connections.ToList()[0].input;
            else if (outputs[i].connections.ToList()[0].output != outputs[i])
                otherport = outputs[i].connections.ToList()[0].output;
            else
            {
                EditorUtility.DisplayDialog("Ошибка", "Ошибка привязки нодов и ивентов", "ok");
                return;
            }

            ActionNodeBase node = otherport.node as ActionNodeBase;

            action.NextActions.Add(node.action);
        }
    }
}
