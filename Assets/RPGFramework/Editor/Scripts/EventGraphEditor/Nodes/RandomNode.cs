using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class RandomNode : ActionNodeBase
{
    public RandomNode(RandomAction action) : base(action)
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
        RandomAction ca = action as RandomAction;

        Slider ChanceSlider = new Slider("Шанс", 0f, 1f);

        ChanceSlider.SetValueWithoutNotify(ca.Chance);
        ChanceSlider.RegisterValueChangedCallback(i =>
        {
            ca.Chance = i.newValue;

            MakeDirty();
        });

        extensionContainer.Add(ChanceSlider);
    }
}
