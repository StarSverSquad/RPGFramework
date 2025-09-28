using RPGF;
using RPGF.Core.Location;
using RPGF.Domain;
using RPGF.Editor.EventSystem;
using RPGF.Editor.EventSystem.Attributes;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[UseActionNodeAttribute]
public class LocationTrasmitionNode : ActionNodeBase<LocationTrasmitionAction>
{
    public LocationTrasmitionNode(LocationTrasmitionAction Action) : base(Action)
    {
    }

    public override void UIContructor()
    {
        ObjectField locationField = new ObjectField("Локация")
        {
            objectType = typeof(RpgfLocationInfo),
            allowSceneObjects = false
        };

        locationField.SetValueWithoutNotify(Action.Message.Location);
        locationField.RegisterValueChangedCallback(value =>
        {
            Action.Message.Location = (RpgfLocationInfo)value.newValue;

            MakeDirty();
        });

        TextField spawnPointField = new TextField("Точка спавна");

        spawnPointField.SetValueWithoutNotify(Action.Message.Point);
        spawnPointField.RegisterValueChangedCallback(value =>
        {
            Action.Message.Point = value.newValue;

            MakeDirty();
        });

        EnumField enumField = new EnumField("Направление", Action.Message.Direction);

        enumField.RegisterValueChangedCallback(data =>
        {
            Action.Message.Direction = (ViewDirection)data.newValue;

            MakeDirty();
        });

        extensionContainer.Add(locationField);
        extensionContainer.Add(spawnPointField);
        extensionContainer.Add(enumField);
    }
}