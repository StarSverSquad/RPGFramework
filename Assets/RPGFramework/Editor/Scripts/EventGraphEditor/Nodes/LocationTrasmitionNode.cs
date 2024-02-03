using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class LocationTrasmitionNode : ActionNodeBase
{
    public LocationTrasmitionNode(LocationTrasmitionAction action) : base(action)
    {
    }

    public override void UIContructor()
    {
        LocationTrasmitionAction act = action as LocationTrasmitionAction;

        ObjectField locationField = new ObjectField("Локация")
        {
            objectType = typeof(LocationInfo),
            allowSceneObjects = false
        };

        locationField.SetValueWithoutNotify(act.Location);
        locationField.RegisterValueChangedCallback(value =>
        {
            act.Location = (LocationInfo)value.newValue;
        });

        TextField spawnPointField = new TextField("Точка спавна");

        spawnPointField.SetValueWithoutNotify(act.SpawnPointName);
        spawnPointField.RegisterValueChangedCallback(value =>
        {
            act.SpawnPointName = value.newValue;
        });

        extensionContainer.Add(locationField);
        extensionContainer.Add(spawnPointField);
    }
}