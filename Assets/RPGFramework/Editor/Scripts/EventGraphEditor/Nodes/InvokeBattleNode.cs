using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor.UIElements;

public class InvokeBattleNode : ActionNodeBase
{
    public InvokeBattleNode(InvokeBattleAction action) : base(action)
    {

    }

    public override void PortContructor()
    {
        CreateInputPort("Input");

        CreateOutputPort("Then");

        InvokeBattleAction ib = (InvokeBattleAction)action;

        if (ib.fleePort)
        {
            CreateOutputPort("Flee", Color.yellow);
        }

        if (ib.battle != null && ib.battle.CanLose)
        {
            CreateOutputPort("Lose", Color.red);
        }
    }

    public override void UIContructor()
    {
        InvokeBattleAction ib = (InvokeBattleAction)action;

        ObjectField battleField = new ObjectField("Битва")
        {
            objectType = typeof(RPGBattleInfo),
            allowSceneObjects = false
        };

        battleField.SetValueWithoutNotify(ib.battle);
        battleField.RegisterValueChangedCallback(val =>
        {
            ib.battle = (RPGBattleInfo)val.newValue;

            UpdatePorts();

            MakeDirty();
        });

        extensionContainer.Add(battleField);

        Toggle fleeToggle = new Toggle("Считать побег?");

        fleeToggle.SetValueWithoutNotify(ib.fleePort);
        fleeToggle.RegisterValueChangedCallback(val =>
        {
            ib.fleePort = val.newValue;

            UpdatePorts();

            MakeDirty();
        });

        extensionContainer.Add(fleeToggle);
    }
}