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

        locationField.SetValueWithoutNotify(act.Message.Location);
        locationField.RegisterValueChangedCallback(value =>
        {
            act.Message.Location = (LocationInfo)value.newValue;

            MakeDirty();
        });

        TextField spawnPointField = new TextField("Точка спавна");

        spawnPointField.SetValueWithoutNotify(act.Message.Point);
        spawnPointField.RegisterValueChangedCallback(value =>
        {
            act.Message.Point = value.newValue;

            MakeDirty();
        });

        EnumField enumField = new EnumField("Направление", act.Message.Direction);

        enumField.RegisterValueChangedCallback(data =>
        {
            act.Message.Direction = (CommonDirection)data.newValue;

            MakeDirty();
        });

        extensionContainer.Add(locationField);
        extensionContainer.Add(spawnPointField);
        extensionContainer.Add(enumField);
    }
}