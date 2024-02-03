using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
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
        /// Начальное название ноды должно полностью сответсвовать названию действия ActAction = ActNode

        ActionNodeBase node;

        Assembly editorAssembly = typeof(ActionNodeBase).Assembly;


        string name = action.GetType().Name.Split("Action")[0];

        Type nodeType = editorAssembly.GetTypes()
            .FirstOrDefault(i => i.BaseType == typeof(ActionNodeBase) && i.Name.StartsWith(name));

        if (nodeType != null)
        {
            node = (ActionNodeBase)Activator.CreateInstance(nodeType, new object[] { action });
        }
        else
        {
            EditorUtility.DisplayDialog("Ошибка", $"Ноды для действия {action.GetType().Name} не существует", "Ok");

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

        if (evt.target is ActionNodeBase node)
        {
            if (node is not StartNode)
            {
                object obj = node.action.Clone() as GraphActionBase;

                GraphActionBase action;

                if (obj is GraphActionBase act)
                    action = act;
                else
                    action = node.action.GetType().Assembly.CreateInstance(node.action.GetType().Name) as GraphActionBase;

                evt.menu.AppendAction("Создать дубликат", i => CreateNode(action, mousePosition));
            }
        }

        evt.menu.AppendAction("Диалог/Сообщение", i => CreateNode(new MessageAction(), mousePosition));
        evt.menu.AppendAction("Диалог/Выбор", i => CreateNode(new ChoiceAction(), mousePosition));

        evt.menu.AppendAction("Ветвление/Условие", i => CreateNode(new ConditionAction(), mousePosition));
        evt.menu.AppendAction("Ветвление/Управление переменной", i => CreateNode(new ManageVarAction(), mousePosition));
        
        evt.menu.AppendAction("Партия/Изменить состав команды", i => CreateNode(new AddRemoveCharacterAction(), mousePosition));

        evt.menu.AppendAction("События исследования/Управление BGM", i => CreateNode(new ManageBGMAction(), mousePosition));
        evt.menu.AppendAction("События исследования/Управление BGS", i => CreateNode(new ManageBGSAction(), mousePosition));
        evt.menu.AppendAction("События исследования/Запуск SE", i => CreateNode(new PlaySEAction(), mousePosition));
        evt.menu.AppendAction("События исследования/Запуск ME", i => CreateNode(new PlayMEAction(), mousePosition));
        evt.menu.AppendAction("События исследования/Запуск самопис. события", i => CreateNode(new InvokeCustomAction(), mousePosition));
        evt.menu.AppendAction("События исследования/Настройка солнечного света", i => CreateNode(new SetupSunLightAction(), mousePosition));
        evt.menu.AppendAction("События исследования/Смена локации", i => CreateNode(new LocationTrasmitionAction(), mousePosition));

        evt.menu.AppendAction("События битвы/Битва", i => CreateNode(new InvokeBattleAction(), mousePosition));

        evt.menu.AppendAction("Разное/Ждать", i => CreateNode(new WaitAction(), mousePosition));
        evt.menu.AppendAction("Разное/Вкл.\\Выкл. объект", i => CreateNode(new SetActiveAction(), mousePosition));

        evt.menu.AppendAction("Конец", i => CreateNode(new EndAction(), mousePosition));
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

            if (item.input == null || item.output == null ||
                left == null || right == null)
                continue;

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
