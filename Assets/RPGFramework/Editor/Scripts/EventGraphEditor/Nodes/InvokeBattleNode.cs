using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor.UIElements;

[UseActionNode]
public class InvokeBattleNode : ActionNodeWrapper<InvokeBattleAction>
{
    public InvokeBattleNode(InvokeBattleAction Action) : base(Action)
    {

    }

    public override void PortContructor()
    {
        CreateInputPort("Input");

        CreateOutputPort("Then");

        if (Action.fleePort)
        {
            CreateOutputPort("Flee", Color.yellow);
        }

        if (Action.battle != null && Action.battle.CanLose)
        {
            CreateOutputPort("Lose", Color.red);
        }
    }

    public override void UIContructor()
    {
        ObjectField battleField = new ObjectField("Битва")
        {
            objectType = typeof(RPGBattleInfo),
            allowSceneObjects = false
        };

        battleField.SetValueWithoutNotify(Action.battle);
        battleField.RegisterValueChangedCallback(val =>
        {
            Action.battle = (RPGBattleInfo)val.newValue;

            UpdatePorts();

            MakeDirty();
        });

        extensionContainer.Add(battleField);

        Toggle fleeToggle = new Toggle("Считать побег?");

        fleeToggle.SetValueWithoutNotify(Action.fleePort);
        fleeToggle.RegisterValueChangedCallback(val =>
        {
            Action.fleePort = val.newValue;

            UpdatePorts();

            MakeDirty();
        });

        extensionContainer.Add(fleeToggle);
    }
}