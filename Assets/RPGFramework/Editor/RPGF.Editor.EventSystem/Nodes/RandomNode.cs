using RPGF.Actions;
using RPGF.Editor.EventSystem;
using RPGF.Editor.EventSystem.Attributes;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPGF.Editor.EventSystem.Nodes
{
    [UseActionNode("Случайное исход", contextualMenuPath: "Ветвление/Случайное исход")]
    public class RandomNode : ActionNodeBase<RandomAction>
    {
        public RandomNode(RandomAction Action) : base(Action)
        {
            extensionContainer.style.backgroundColor = (Color)(new Color32(77, 77, 77, 255));
            extensionContainer.style.minWidth = 200;
        }

        public override void PortContructor()
        {
            CreateInputPort("Вход", "Input");

            CreateOutputPort("Да", RandomAction.YES_NextTag, new Color32(17, 156, 56, 255), RandomAction.YES_NextTag);
            CreateOutputPort("Нет", RandomAction.NO_NextTag, new Color32(156, 17, 17, 255), RandomAction.NO_NextTag);
        }

        public override void UIContructor()
        {
            Slider ChanceSlider = new("Шанс", 0f, 1f);

            ChanceSlider.SetValueWithoutNotify(Action.Chance);
            ChanceSlider.RegisterValueChangedCallback(i =>
            {
                Action.Chance = i.newValue;

                MakeDirty();
            });

            extensionContainer.Add(ChanceSlider);
        }
    }

}