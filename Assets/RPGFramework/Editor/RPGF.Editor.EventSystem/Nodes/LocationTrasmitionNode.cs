using RPGF.Actions;
using RPGF.Editor.EventSystem.Attributes;

namespace RPGF.Editor.EventSystem.Nodes
{
    [UseActionNode("Смена локации", contextualMenuPath: "Система/Смена локации")]
    public class LocationTrasmitionNode : ActionNodeBase<LocationTrasmitionAction>
    {
        public LocationTrasmitionNode(LocationTrasmitionAction Action) : base(Action)
        {
        }

        public override void UIContructor()
        {
            var locationField = BuildObjectField(Action.Dto.Location, value =>
            {
                Action.Dto.Location = value;
            }, "Локация");

            var spawnPointField = BuildTextField(Action.Dto.Point, value =>
            {
                Action.Dto.Point = value;
            }, "Точка спавна");

            var enumField = BuildEnumField(Action.Dto.Direction, value =>
            {
                Action.Dto.Direction = value;
            }, "Направление");

            extensionContainer.Add(locationField);
            extensionContainer.Add(spawnPointField);
            extensionContainer.Add(enumField);
        }
    }
}