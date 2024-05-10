using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ChangeBattleBGMNode : ActionNodeBase
{
    public ChangeBattleBGMNode(ChangeBattleBGMAction action) : base(action)
    {
    }

    public override void UIContructor()
    {
        ChangeBattleBGMAction se = action as ChangeBattleBGMAction;

        ObjectField clipField = new ObjectField("Трек")
        {
            allowSceneObjects = false,
            objectType = typeof(AudioClip)
        };

        clipField.SetValueWithoutNotify(se.Clip);
        clipField.RegisterValueChangedCallback(i =>
        {
            se.Clip = i.newValue as AudioClip;

            MakeDirty();
        });

        Slider slider = new Slider("Громкость", 0f, 1f);

        slider.SetValueWithoutNotify(se.Volume);
        slider.RegisterValueChangedCallback(i =>
        {
            se.Volume = i.newValue;

            MakeDirty();
        });

        extensionContainer.Add(clipField);
        extensionContainer.Add(slider);
    }
}
