using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[UseActionNode]
public class PlaySENode : ActionNodeWrapper<PlaySEAction>
{
    public PlaySENode(PlaySEAction Action) : base(Action)
    {
    }

    public override void UIContructor()
    {
        ObjectField clipField = new ObjectField("Аудио")
        {
            allowSceneObjects = true,
            objectType = typeof(AudioClip)
        };

        clipField.SetValueWithoutNotify(Action.clip);
        clipField.RegisterValueChangedCallback(clip =>
        {
            Action.clip = clip.newValue as AudioClip;

            MakeDirty();
        });

        FloatField volField = new FloatField("Громкость");

        volField.SetValueWithoutNotify(Action.volume);
        volField.RegisterValueChangedCallback(vol =>
        {
            Action.volume = vol.newValue;

            MakeDirty();
        });

        extensionContainer.Add(clipField);
        extensionContainer.Add(volField);
    }
}
