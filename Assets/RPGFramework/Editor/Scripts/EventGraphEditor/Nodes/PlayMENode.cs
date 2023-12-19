using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayMENode : ActionNodeBase
{
    public PlayMENode(PlayMEAction action) : base(action)
    {
    }

    public override void UIContructor()
    {
        PlayMEAction se = action as PlayMEAction;

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