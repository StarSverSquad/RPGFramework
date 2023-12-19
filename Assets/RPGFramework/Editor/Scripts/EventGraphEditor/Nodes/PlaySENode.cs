using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class PlaySENode : ActionNodeBase
{
    public PlaySENode(PlaySEAction action) : base(action)
    {
    }

    public override void UIContructor()
    {
        PlaySEAction se = action as PlaySEAction;

        ObjectField clipField = new ObjectField("Аудио")
        {
            allowSceneObjects = true,
            objectType = typeof(AudioClip)
        };

        clipField.SetValueWithoutNotify(se.clip);
        clipField.RegisterValueChangedCallback(clip =>
        {
            se.clip = clip.newValue as AudioClip;

            MakeDirty();
        });

        FloatField volField = new FloatField("Громкость");

        volField.SetValueWithoutNotify(se.volume);
        volField.RegisterValueChangedCallback(vol =>
        {
            se.volume = vol.newValue;

            MakeDirty();
        });

        extensionContainer.Add(clipField);
        extensionContainer.Add(volField);
    }
}
