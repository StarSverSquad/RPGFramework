using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class SetupSunLightNode : ActionNodeBase
{
    public SetupSunLightNode(SetupSunLightAction action) : base(action)
    {
    }

    public override void UIContructor()
    {
        SetupSunLightAction act = action as SetupSunLightAction;

        FloatField intensityField = new FloatField("Интенсивность");

        intensityField.SetValueWithoutNotify(act.Intensity);

        intensityField.RegisterValueChangedCallback(value =>
        {
            if (value.newValue < 0)
            {
                act.Intensity = 0;
                intensityField.SetValueWithoutNotify(0);
            }  
            else
                act.Intensity = value.newValue;

            MakeDirty();
        });

        ColorField colorField = new ColorField("Цвет света");

        colorField.SetValueWithoutNotify(act.Color);

        colorField.RegisterValueChangedCallback(value =>
        {
            act.Color = value.newValue;

            MakeDirty();
        });

        extensionContainer.Add(intensityField);
        extensionContainer.Add(colorField);
    }
}