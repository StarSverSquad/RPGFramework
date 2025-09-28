using RPGF.Editor.EventSystem;
using RPGF.Editor.EventSystem.Attributes;
using UnityEngine;
using UnityEngine.UIElements;

[UseActionNodeAttribute]
public class RandomNode : ActionNodeBase<RandomAction>
{
    public RandomNode(RandomAction Action) : base(Action)
    {
        extensionContainer.style.backgroundColor = (Color)(new Color32(77, 77, 77, 255));
        extensionContainer.style.minWidth = 200;
    }

    public override void PortContructor()
    {
        CreateInputPort("Input");

        CreateOutputPort("Yes", new Color32(17, 156, 56, 255));
        CreateOutputPort("No", new Color32(156, 17, 17, 255));
    }

    public override void UIContructor()
    {
        Slider ChanceSlider = new Slider("Шанс", 0f, 1f);

        ChanceSlider.SetValueWithoutNotify(Action.Chance);
        ChanceSlider.RegisterValueChangedCallback(i =>
        {
            Action.Chance = i.newValue;

            MakeDirty();
        });

        extensionContainer.Add(ChanceSlider);
    }
}
