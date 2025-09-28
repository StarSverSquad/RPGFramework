using RPGF.Editor.EventSystem;
using RPGF.Editor.EventSystem.Attributes;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[UseActionNodeAttribute]
public class ChangeBattleBGMNode : ActionNodeBase<ChangeBattleBGMAction>
{
    public ChangeBattleBGMNode(ChangeBattleBGMAction Action) : base(Action)
    {
    }

    public override void UIContructor()
    {
        ObjectField clipField = new ObjectField("Трек")
        {
            allowSceneObjects = false,
            objectType = typeof(AudioClip)
        };

        clipField.SetValueWithoutNotify(Action.Clip);
        clipField.RegisterValueChangedCallback(i =>
        {
            Action.Clip = i.newValue as AudioClip;

            MakeDirty();
        });

        Slider slider = new Slider("Громкость", 0f, 1f);

        slider.SetValueWithoutNotify(Action.Volume);
        slider.RegisterValueChangedCallback(i =>
        {
            Action.Volume = i.newValue;

            MakeDirty();
        });

        extensionContainer.Add(clipField);
        extensionContainer.Add(slider);
    }
}
