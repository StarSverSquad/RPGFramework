using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ManageBGSNode : ActionNodeBase<ManageBGSAction>
{
    public ManageBGSNode(ManageBGSAction action) : base(action)
    {
    }

    private string Formater(int i)
    {
        return i switch
        {
            0 => "Запустить",
            1 => "Пауза",
            2 => "Остановить",
            3 => "Изменить громкость",
            4 => "Возобновить",
            _ => "UNDEF"
        };
    }

    public override void UIContructor()
    {
        Label lbl = new Label("Тип операции");

        extensionContainer.Add(lbl);

        PopupField<int> popupField = new PopupField<int>(new List<int> { 0, 1, 2, 3, 4 }, 0, Formater, Formater);

        popupField.SetValueWithoutNotify((int)action.Operation);
        popupField.RegisterValueChangedCallback(i =>
        {
            action.Operation = (ManageBGSAction.OperationType)i.newValue;

            UpdateUI();

            MakeDirty();
        });

        extensionContainer.Add(popupField);

        FloatField volumeField = new FloatField("Громкость");

        volumeField.SetValueWithoutNotify(action.Volume);
        volumeField.RegisterValueChangedCallback(i =>
        {
            action.Volume = i.newValue;

            MakeDirty();
        });

        Toggle fadeToggle = new Toggle("Использовать затухание/появление?");

        fadeToggle.SetValueWithoutNotify(action.UseFade);
        fadeToggle.RegisterValueChangedCallback(i =>
        {
            action.UseFade = i.newValue;

            UpdateUI();

            MakeDirty();
        });

        Toggle waitFadeToggle = new Toggle("Ждать затухание/появление?");

        waitFadeToggle.SetValueWithoutNotify(action.WaitFade);
        waitFadeToggle.RegisterValueChangedCallback(i =>
        {
            action.WaitFade = i.newValue;

            MakeDirty();
        });

        FloatField fadeTimeField = new FloatField("Время появления/затухания");

        fadeTimeField.SetValueWithoutNotify(action.FadeTime);
        fadeTimeField.RegisterValueChangedCallback(i =>
        {
            action.FadeTime = i.newValue;

            MakeDirty();
        });

        switch (action.Operation)
        {
            case ManageBGSAction.OperationType.Play:

                ObjectField clipField = new ObjectField("Аудио")
                {
                    objectType = typeof(AudioClip),
                    allowSceneObjects = true
                };

                clipField.SetValueWithoutNotify(action.clip);
                clipField.RegisterValueChangedCallback(i =>
                {
                    action.clip = (AudioClip)i.newValue;

                    MakeDirty();
                });

                Toggle ingoreToggle = new Toggle("Пропуск если запущет то же аудио?");

                ingoreToggle.SetValueWithoutNotify(action.IngoreIfThisClip);
                ingoreToggle.RegisterValueChangedCallback(i =>
                {
                    action.IngoreIfThisClip = i.newValue;

                    MakeDirty();
                });

                extensionContainer.Add(clipField);
                extensionContainer.Add(volumeField);
                extensionContainer.Add(ingoreToggle);
                extensionContainer.Add(fadeToggle);

                if (action.UseFade)
                {
                    extensionContainer.Add(fadeTimeField);
                    extensionContainer.Add(waitFadeToggle);
                }                   
                break;
            case ManageBGSAction.OperationType.Pause:
                extensionContainer.Add(fadeToggle);

                if (action.UseFade)
                {
                    extensionContainer.Add(fadeTimeField);
                    extensionContainer.Add(waitFadeToggle);
                }
                break;
            case ManageBGSAction.OperationType.Stop:
                extensionContainer.Add(fadeToggle);

                if (action.UseFade)
                {
                    extensionContainer.Add(fadeTimeField);
                    extensionContainer.Add(waitFadeToggle);
                }
                break;
            case ManageBGSAction.OperationType.VolumeChange:
                extensionContainer.Add(volumeField);
                extensionContainer.Add(fadeToggle);

                if (action.UseFade)
                {
                    extensionContainer.Add(fadeTimeField);
                    extensionContainer.Add(waitFadeToggle);
                }
                break;
            case ManageBGSAction.OperationType.Resume:
                extensionContainer.Add(volumeField);
                extensionContainer.Add(fadeToggle);

                if (action.UseFade)
                {
                    extensionContainer.Add(fadeTimeField);
                    extensionContainer.Add(waitFadeToggle);
                }
                break;
        }
    }
}
