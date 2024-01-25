using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ManageBGMNode : ActionNodeBase
{
    public ManageBGMNode(ManageBGMAction action) : base(action)
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
        ManageBGMAction ma = action as ManageBGMAction;

        Label lbl = new Label("Тип операции");

        extensionContainer.Add(lbl);

        PopupField<int> popupField = new PopupField<int>(new List<int> { 0, 1, 2, 3, 4 }, 0, Formater, Formater);

        popupField.SetValueWithoutNotify((int)ma.Operation);
        popupField.RegisterValueChangedCallback(i =>
        {
            ma.Operation = (ManageBGMAction.OperationType)i.newValue;

            UpdateUI();

            MakeDirty();
        });

        extensionContainer.Add(popupField);

        FloatField volumeField = new FloatField("Громкость");

        volumeField.SetValueWithoutNotify(ma.Volume);
        volumeField.RegisterValueChangedCallback(i =>
        {
            ma.Volume = i.newValue;

            MakeDirty();
        });

        Toggle fadeToggle = new Toggle("Использовать затухание/появление?");

        fadeToggle.SetValueWithoutNotify(ma.UseFade);
        fadeToggle.RegisterValueChangedCallback(i =>
        {
            ma.UseFade = i.newValue;

            UpdateUI();

            MakeDirty();
        });

        Toggle waitFadeToggle = new Toggle("Ждать затухание/появление?");

        waitFadeToggle.SetValueWithoutNotify(ma.WaitFade);
        waitFadeToggle.RegisterValueChangedCallback(i =>
        {
            ma.WaitFade = i.newValue;

            MakeDirty();
        });

        FloatField fadeTimeField = new FloatField("Время появления/затухания");

        fadeTimeField.SetValueWithoutNotify(ma.FadeTime);
        fadeTimeField.RegisterValueChangedCallback(i =>
        {
            ma.FadeTime = i.newValue;

            MakeDirty();
        });

        switch (ma.Operation)
        {
            case ManageBGMAction.OperationType.Play:

                ObjectField clipField = new ObjectField("Аудио")
                {
                    objectType = typeof(AudioClip),
                    allowSceneObjects = true
                };

                clipField.SetValueWithoutNotify(ma.clip);
                clipField.RegisterValueChangedCallback(i =>
                {
                    ma.clip = (AudioClip)i.newValue;

                    MakeDirty();
                });

                Toggle ingoreToggle = new Toggle("Пропуск если запущет то же аудио?");

                ingoreToggle.SetValueWithoutNotify(ma.IngoreIfThisClip);
                ingoreToggle.RegisterValueChangedCallback(i =>
                {
                    ma.IngoreIfThisClip = i.newValue;

                    MakeDirty();
                });

                extensionContainer.Add(clipField);
                extensionContainer.Add(volumeField);
                extensionContainer.Add(ingoreToggle);
                extensionContainer.Add(fadeToggle);

                if (ma.UseFade)
                {
                    extensionContainer.Add(fadeTimeField);
                    extensionContainer.Add(waitFadeToggle);
                }                   
                break;
            case ManageBGMAction.OperationType.Pause:
                extensionContainer.Add(fadeToggle);

                if (ma.UseFade)
                {
                    extensionContainer.Add(fadeTimeField);
                    extensionContainer.Add(waitFadeToggle);
                }
                break;
            case ManageBGMAction.OperationType.Stop:
                extensionContainer.Add(fadeToggle);

                if (ma.UseFade)
                {
                    extensionContainer.Add(fadeTimeField);
                    extensionContainer.Add(waitFadeToggle);
                }
                break;
            case ManageBGMAction.OperationType.VolumeChange:
                extensionContainer.Add(volumeField);
                extensionContainer.Add(fadeToggle);

                if (ma.UseFade)
                {
                    extensionContainer.Add(fadeTimeField);
                    extensionContainer.Add(waitFadeToggle);
                }
                break;
            case ManageBGMAction.OperationType.Resume:
                extensionContainer.Add(volumeField);
                extensionContainer.Add(fadeToggle);

                if (ma.UseFade)
                {
                    extensionContainer.Add(fadeTimeField);
                    extensionContainer.Add(waitFadeToggle);
                }
                break;
        }
    }
}
