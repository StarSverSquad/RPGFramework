using RPGF;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[UseActionNode]
public class LocationTrasmitionNode : ActionNodeWrapper<LocationTrasmitionAction>
{
    public LocationTrasmitionNode(LocationTrasmitionAction Action) : base(Action)
    {
    }

    public override void UIContructor()
    {
        ObjectField locationField = new ObjectField("Локация")
        {
            objectType = typeof(LocationInfo),
            allowSceneObjects = false
        };

        locationField.SetValueWithoutNotify(Action.Message.Location);
        locationField.RegisterValueChangedCallback(value =>
        {
            Action.Message.Location = (LocationInfo)value.newValue;

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