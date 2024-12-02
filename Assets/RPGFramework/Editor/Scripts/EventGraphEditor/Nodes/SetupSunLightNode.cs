using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class SetupSunLightNode : ActionNodeWrapper<SetupSunLightAction>
{
    public SetupSunLightNode(SetupSunLightAction Action) : base(Action)
    {
    }

    public override void UIContructor()
    {
        FloatField intensityField = new FloatField("Интенсивность");

        intensityField.SetValueWithoutNotify(Action.Intensity);

        intensityField.RegisterValueChangedCallback(value =>
        {
            if (value.newValue < 0)
            {
                Action.Intensity = 0;
                intensityField.SetValueWithoutNotify(0);
            }  
            else
                Action.Intensity = value.newValue;

            MakeDirty();
        });

        ColorField colorField = new ColorField("Цвет света");

        colorField.SetValueWithoutNotify(Action.Color);

        colorField.RegisterValueChangedCallback(value =>
        {
            Action.Color = value.newValue;

            MakeDirty();
        });

        extensionContainer.Add(intensityField);
        extensionContainer.Add(colorField);
    }
}