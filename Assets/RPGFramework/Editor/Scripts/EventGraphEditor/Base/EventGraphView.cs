using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class EventGraphView : GraphView
{
    public GraphEvent GEvent;

    public readonly Vector2 DefaultNodeSize = new Vector2(300, 400);

    public event Action OnMakeDirty;
    public event Action OnSaved;

    public EventGraphView(GraphEvent gEvent)
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new ContentZoomer());

        var grid = new GridBackground();
        styleSheets.Add(Resources.Load<StyleSheet>("EventGraphViewStyle"));

        Insert(0, grid);

        grid.StretchToParentSize();
        GEvent = gEvent;

        if (GEvent.Actions.Count > 0)
            LoadGraph();
        else
            CreateNode(new StartAction(), new Vector2(250, 250));
    }

    public override EventPropagation DeleteSelection()
    {
        foreach (var item in selection.OfType<Node>())
        {
            if (item is StartNode)
                return EventPropagation.Stop;
        }

        MakeDirty();

        base.DeleteSelection();

        return EventPropagation.Continue;
    }

    public void CreateNode(GraphActionBase action, Vector2 position, string guid = null)
    {
        ActionNodeBase node;
        switch (action)
        {
            case StartAction a:
                node = new StartNode(a);
                break;
            case EndAction a:
                node = new EndNode(a);              
                break;
            case ConditionAction a:
                node = new ConditionNode(a);
                break;
            case DebugAction a:
                node = new DebugNode(a);
                break;
            case ManageBGMAction a:
                node = new ManageBGMNode(a);
                break;
            case ManageBGSAction a:
                node = new ManageBGSNode(a);
                break;
            case PlaySEAction a:
                node = new PlaySENode(a);
                break;
            case PlayMEAction a:
                node = new PlayMENode(a);
                break;
            case WaitAction a:
                node = new WaitNode(a);
                break;
            case MessageAction a:
                node = new MessageNode(a);
                break;
            case ChoiceAction a:
                node = new ChoiceNode(a);
                break;
            case ManageVarAction a:
                node = new ManageVarNode(a);
                break;
            case InvokeCustomAction a:
                node = new InvokeCustomNode(a);
                break;
            default:
                EditorUtility.DisplayDialog("Ошибка", $"Нода под событие {action.Name} не существует", "Ok");
                return;
        }

        node.view = this;

        node.GUID = guid ?? GUID.Generate().ToString();

        node.PortContructor();
        node.UIContructor();

        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(position, DefaultNodeSize));

        AddElement(node);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> result = new List<Port>();

        ports.ForEach(port =>
        {
            if (startPort != port && startPort.node != port.node && startPort.direction != port.direction
                && startPort.portType == port.portType)
                result.Add(port);
        });

        return result;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        base.BuildContextualMenu(evt);

        Vector2 mousePosition = this.ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);

        evt.menu.AppendSeparator("Общие события");
        evt.menu.AppendSeparator("События битвы");
        evt.menu.AppendSeparator("События исследования");

        evt.menu.AppendAction("Общие События/Сообщение", i => CreateNode(new MessageAction(), mousePosition));
        evt.menu.AppendAction("Общие События/Выбор", i => CreateNode(new ChoiceAction(), mousePosition));
        evt.menu.AppendAction("Общие События/Условие", i => CreateNode(new ConditionAction(), mousePosition));
        evt.menu.AppendAction("Общие События/Управление переменной", i => CreateNode(new ManageVarAction(), mousePosition));
        evt.menu.AppendAction("Общие События/Ждать", i => CreateNode(new WaitAction(), mousePosition));
        evt.menu.AppendAction("Общие События/Конец", i => CreateNode(new EndAction(), mousePosition));

        evt.menu.AppendAction("События исследования/Управление BGM", i => CreateNode(new ManageBGMAction(), mousePosition));
        evt.menu.AppendAction("События исследования/Управление BGS", i => CreateNode(new ManageBGSAction(), mousePosition));
        evt.menu.AppendAction("События исследования/Запуск SE", i => CreateNode(new PlaySEAction(), mousePosition));
        evt.menu.AppendAction("События исследования/Запуск ME", i => CreateNode(new PlayMEAction(), mousePosition));
        evt.menu.AppendAction("События исследования/Запуск самопис. события", i => CreateNode(new InvokeCustomAction(), mousePosition));

        evt.menu.AppendAction("Отладочное событие", i => CreateNode(new DebugAction(), mousePosition));
    }

    public void MakeDirty()
    {
        OnMakeDirty?.Invoke();
    }

    public void SaveGraph()
    {
        GEvent.Meta.nodes.Clear();
        GEvent.Meta.edges.Clear();
        GEvent.Actions.Clear();

        foreach (var item in nodes)
        {
            ActionNodeBase ban = item as ActionNodeBase;

            GEvent.Actions.Add(ban.action);
        }

        foreach (var item in nodes)
        {
            ActionNodeBase ban = item as ActionNodeBase;

            ban.ApplyPorts();

            GEvent.Meta.nodes.Add(new GraphEventMeta.NodeMeta
            {
                guid = ban.GUID,
                position = ban.localBound.position,
                actionIndex = GEvent.Actions.IndexOf(ban.action)
            });
        }

        foreach (var item in edges)
        {
            ActionNodeBase left = item.output.node as ActionNodeBase;
            ActionNodeBase right = item.input.node as ActionNodeBase;

            GEvent.Meta.edges.Add(new GraphEventMeta.EdgeMeta
            {
                inputNodeGUID = right.GUID,
                outputNodeGUID = left.GUID,
                inputPortName = item.input.portName,
                outputPortName = item.output.portName
            });
        }

        EditorUtility.SetDirty(GEvent);
        GEvent.DispathGraphChanges();

        OnSaved?.Invoke();
    }
    public void LoadGraph()
    {
        foreach (var node in GEvent.Meta.nodes)
        {
            CreateNode(GEvent.Actions[node.actionIndex] as GraphActionBase, node.position, node.guid);
        }

        foreach (var item in nodes)
        {
            ActionNodeBase ban = item as ActionNodeBase;

            List<GraphEventMeta.EdgeMeta> tedges = GEvent.Meta.edges.Where(i => i.outputNodeGUID == ban.GUID).ToList();

            List<Port> tports = ports.Where(i => i.node == item && i.direction == Direction.Output).ToList();

            foreach (var port in tports)
            {
                foreach (var edge in tedges)
                {
                    if (port.portName != edge.outputPortName)
                        continue;

                    Node ohter = nodes.First(i =>
                    {
                        ActionNodeBase ban0 = i as ActionNodeBase;

                        return edge.inputNodeGUID == ban0.GUID;
                    });

                    Port otherport = ports.First(i => i.node == ohter && i.portName == edge.inputPortName);

                    Edge nedge = new Edge
                    {
                        input = otherport,
                        output = port
                    };

                    port.Connect(nedge);
                    otherport.Connect(nedge);

                    Add(nedge);
                }
            }
        }
    }
}
