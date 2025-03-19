using RPGF.RPG;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[UseActionNode]
public class ConditionNode : ActionNodeWrapper<ConditionAction>
{
    public enum ConditionType
    {
        IntVar, FloatVar, BoolVar, StringVar, Money
    }

    private int lastConditionListIndex = 0;

    public ConditionNode(ConditionAction Action) : base(Action)
    {
        extensionContainer.style.backgroundColor = (Color)(new Color32(77, 77, 77, 255));
        extensionContainer.style.minWidth = 200;
    }

    public string FormatOperationList(int index)
    {
        return index switch
        {
            0 => "==",
            1 => "!=",
            2 => ">",
            3 => "<",
            4 => ">=",
            5 => "<=",
            _ => "UNDEF",
        };
    }

    public override void PortContructor()
    {
        CreateInputPort("Input");

        CreateOutputPort("Then", new Color32(17, 156, 56, 255));
        CreateOutputPort("Else", new Color32(156, 17, 17, 255));
    }

    public override void UIContructor()
    {
        List<string> contypes = Action.GetType().Assembly.GetTypes()
            .Where(i => i.BaseType != null && i.BaseType.Name == "ConditionBase")
            .Select(i => i.Name)
            .ToList();

        PopupField<string> typePopup = new PopupField<string>(contypes, lastConditionListIndex, LabelFormater, LabelFormater);


        Button addButton = new Button(() =>
        {
            Action.Conditions.Add(Action.GetType().Assembly.CreateInstance(typePopup.value) as ConditionBase);

            lastConditionListIndex = typePopup.index;

            UpdateUI();

            MakeDirty();
        })
        {
            text = "Добавить условие"
        };

        extensionContainer.Add(typePopup);
        extensionContainer.Add(addButton);

        foreach (ConditionBase item in Action.Conditions)
        {
            VisualElement conditionBlock = new VisualElement();
            
            conditionBlock.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Column);
            conditionBlock.style.backgroundColor = (Color)new Color32(69, 69, 69, 255);
            conditionBlock.style.marginBottom = 5;

            VisualElement labelHorizontal = new VisualElement();
            labelHorizontal.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            labelHorizontal.style.justifyContent = new StyleEnum<Justify>(Justify.Center);

            Label label = new Label(item.GetLabel());

            labelHorizontal.Add(label);

            conditionBlock.Add(labelHorizontal);

            switch (item)
            {
                case BoolVarCondition blvar:
                    {
                        VisualElement horizontal0 = new VisualElement();
                        horizontal0.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
                        horizontal0.style.justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween);

                        Label lbl0 = new Label("Переменная");
                        Label lbl1 = new Label("Значение");

                        horizontal0.Add(lbl0);
                        horizontal0.Add(lbl1);

                        conditionBlock.Add(horizontal0);

                        VisualElement horizontal1 = new VisualElement();
                        horizontal1.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
                        horizontal1.style.justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween);

                        TextField namefield = new TextField();
                        namefield.SetValueWithoutNotify(blvar.Var);
                        namefield.RegisterValueChangedCallback(i =>
                        {
                            blvar.Var = i.newValue;

                            MakeDirty();
                        });

                        horizontal1.Add(namefield);

                        Label lbl = new Label("==");

                        horizontal1.Add(lbl);

                        Toggle valfield = new Toggle();
                        valfield.SetValueWithoutNotify(blvar.Value);
                        valfield.RegisterValueChangedCallback(i =>
                        {
                            blvar.Value = i.newValue;

                            MakeDirty();
                        });

                        horizontal1.Add(valfield);

                        conditionBlock.Add(horizontal1);
                    }
                    break;
                case StringVarCondition strvar:
                    {
                        VisualElement horizontal0 = new VisualElement();
                        horizontal0.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
                        horizontal0.style.justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween);

                        Label lbl0 = new Label("Переменная");
                        Label lbl1 = new Label("Значение");

                        horizontal0.Add(lbl0);
                        horizontal0.Add(lbl1);

                        conditionBlock.Add(horizontal0);

                        VisualElement horizontal1 = new VisualElement();
                        horizontal1.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
                        horizontal1.style.justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween);

                        TextField namefield = new TextField();
                        namefield.SetValueWithoutNotify(strvar.Var);
                        namefield.RegisterValueChangedCallback(i =>
                        {
                            strvar.Var = i.newValue;

                            MakeDirty();
                        });

                        horizontal1.Add(namefield);

                        Label lbl = new Label("==");

                        horizontal1.Add(lbl);

                        TextField valfield = new TextField();
                        valfield.SetValueWithoutNotify(strvar.Value);
                        valfield.RegisterValueChangedCallback(i =>
                        {
                            strvar.Value = i.newValue;

                            MakeDirty();
                        });

                        horizontal1.Add(valfield);

                        conditionBlock.Add(horizontal1);
                    }
                    break;
                case IntVarCondition intvar:
                    {
                        VisualElement horizontal0 = new VisualElement();
                        horizontal0.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
                        horizontal0.style.justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween);

                        Label lbl0 = new Label("Переменная");
                        Label lbl1 = new Label("Значение");

                        horizontal0.Add(lbl0);
                        horizontal0.Add(lbl1);

                        conditionBlock.Add(horizontal0);

                        VisualElement horizontal1 = new VisualElement();
                        horizontal1.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
                        horizontal1.style.justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween);

                        TextField namefield = new TextField();
                        namefield.SetValueWithoutNotify(intvar.Var);
                        namefield.RegisterValueChangedCallback(i =>
                        {
                            intvar.Var = i.newValue;

                            MakeDirty();
                        });

                        horizontal1.Add(namefield);

                        PopupField<int> popupField = new PopupField<int>(new List<int>() { 0, 1, 2, 3, 4, 5 }, 0, 
                                                                        FormatOperationList, FormatOperationList);

                        popupField.SetValueWithoutNotify((int)intvar.Operation);
                        popupField.RegisterValueChangedCallback(i =>
                        {
                            intvar.Operation = (ConditionOperation)i.newValue;

                            MakeDirty();
                        });

                        horizontal1.Add(popupField);

                        IntegerField valfield = new IntegerField();
                        valfield.SetValueWithoutNotify(intvar.Value);
                        valfield.RegisterValueChangedCallback(i =>
                        {
                            intvar.Value = i.newValue;

                            MakeDirty();
                        });

                        horizontal1.Add(valfield);

                        conditionBlock.Add(horizontal1);
                    }
                    break;
                case FloatVarCondition flvar:
                    {
                        VisualElement horizontal0 = new VisualElement();
                        horizontal0.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
                        horizontal0.style.justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween);

                        Label lbl0 = new Label("Переменная");
                        Label lbl1 = new Label("Значение");

                        horizontal0.Add(lbl0);
                        horizontal0.Add(lbl1);

                        conditionBlock.Add(horizontal0);

                        VisualElement horizontal1 = new VisualElement();
                        horizontal1.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
                        horizontal1.style.justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween);

                        TextField namefield = new TextField();
                        namefield.SetValueWithoutNotify(flvar.Var);
                        namefield.RegisterValueChangedCallback(i =>
                        {
                            flvar.Var = i.newValue;

                            MakeDirty();
                        });

                        horizontal1.Add(namefield);

                        PopupField<int> popupField = new PopupField<int>(new List<int>() { 0, 1, 2, 3, 4, 5 }, 0,
                                                                        FormatOperationList, FormatOperationList);

                        popupField.SetValueWithoutNotify((int)flvar.Operation);
                        popupField.RegisterValueChangedCallback(i =>
                        {
                            flvar.Operation = (ConditionOperation)i.newValue;

                            MakeDirty();
                        });

                        horizontal1.Add(popupField);

                        FloatField valfield = new FloatField();
                        valfield.SetValueWithoutNotify(flvar.Value);
                        valfield.RegisterValueChangedCallback(i =>
                        {
                            flvar.Value = i.newValue;

                            MakeDirty();
                        });

                        horizontal1.Add(valfield);

                        conditionBlock.Add(horizontal1);
                    }
                    break;
                case MoneyCondition mon:
                    {
                        VisualElement horizontal1 = new VisualElement();
                        horizontal1.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
                        horizontal1.style.justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween);

                        Label label1 = new Label("Деньги");
                        label1.style.marginLeft = 1;

                        horizontal1.Add(label1);

                        PopupField<int> popupField = new PopupField<int>(new List<int>() { 0, 1, 2, 3, 4, 5 }, 0,
                                                                        FormatOperationList, FormatOperationList);

                        popupField.SetValueWithoutNotify((int)mon.Operation);
                        popupField.RegisterValueChangedCallback(i =>
                        {
                            mon.Operation = (ConditionOperation)i.newValue;

                            MakeDirty();
                        });

                        horizontal1.Add(popupField);

                        IntegerField valfield = new IntegerField();
                        valfield.SetValueWithoutNotify(mon.Value);
                        valfield.RegisterValueChangedCallback(i =>
                        {
                            mon.Value = i.newValue;

                            MakeDirty();
                        });

                        horizontal1.Add(valfield);

                        conditionBlock.Add(horizontal1);
                    }
                    break;
                case CharacterInPartyCondition cip:
                    {
                        VisualElement horizontal1 = new VisualElement();
                        horizontal1.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
                        horizontal1.style.justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween);

                        Label label1 = new Label("Персонаж");
                        label1.style.marginLeft = 1;

                        horizontal1.Add(label1);

                        Label lbl = new Label("==");

                        horizontal1.Add(lbl);

                        ObjectField charField = new ObjectField()
                        {
                            allowSceneObjects = false,
                            objectType = typeof(RPGCharacter)
                        };
                        charField.SetValueWithoutNotify(cip.Value);
                        charField.RegisterValueChangedCallback(i =>
                        {
                            cip.Value = i.newValue as RPGCharacter;

                            MakeDirty();
                        });

                        horizontal1.Add(charField);

                        conditionBlock.Add(horizontal1);
                    }
                    break;
                case ItemCondition itemcon:
                    {
                        VisualElement horizontal0 = new VisualElement();
                        horizontal0.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
                        horizontal0.style.justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween);

                        Label lbl0 = new Label("Предмет");
                        Label lbl1 = new Label("Количество");

                        horizontal0.Add(lbl0);
                        horizontal0.Add(lbl1);

                        conditionBlock.Add(horizontal0);

                        VisualElement horizontal1 = new VisualElement();
                        horizontal1.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
                        horizontal1.style.justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween);

                        ObjectField itemField = new ObjectField()
                        {
                            allowSceneObjects = false,
                            objectType = typeof(RPGCollectable)
                        };

                        itemField.SetValueWithoutNotify(itemcon.Value);
                        itemField.RegisterValueChangedCallback(i =>
                        {
                            itemcon.Value = i.newValue as RPGCollectable;

                            MakeDirty();
                        });

                        horizontal1.Add(itemField);

                        PopupField<int> popupField = new PopupField<int>(new List<int>() { 0, 1, 2, 3, 4, 5 }, 0,
                                                                        FormatOperationList, FormatOperationList);

                        popupField.SetValueWithoutNotify((int)itemcon.Operation);
                        popupField.RegisterValueChangedCallback(i =>
                        {
                            itemcon.Operation = (ConditionOperation)i.newValue;

                            MakeDirty();
                        });

                        horizontal1.Add(popupField);

                        IntegerField valfield = new IntegerField();
                        valfield.SetValueWithoutNotify(itemcon.Count);
                        valfield.RegisterValueChangedCallback(i =>
                        {
                            if (i.newValue < 0)
                            {
                                valfield.SetValueWithoutNotify(0);
                                itemcon.Count = 0;
                            }
                            else 
                                itemcon.Count = i.newValue;

                            MakeDirty();
                        });

                        horizontal1.Add(valfield);

                        conditionBlock.Add(horizontal1);
                    }
                    break;
                case PlayerPrefsCondition pref:
                    {
                        VisualElement horizontal0 = new VisualElement();
                        horizontal0.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
                        horizontal0.style.justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween);

                        Label lbl0 = new Label("Ключ");
                        Label lbl1 = new Label("Значение");

                        horizontal0.Add(lbl0);
                        horizontal0.Add(lbl1);

                        conditionBlock.Add(horizontal0);

                        VisualElement horizontal1 = new VisualElement();
                        horizontal1.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
                        horizontal1.style.justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween);

                        TextField namefield = new TextField();
                        namefield.SetValueWithoutNotify(pref.Key);
                        namefield.RegisterValueChangedCallback(i =>
                        {
                            pref.Key = i.newValue;

                            MakeDirty();
                        });

                        horizontal1.Add(namefield);

                        Label lbl = new Label("==");

                        horizontal1.Add(lbl);

                        IntegerField charField = new IntegerField();

                        charField.SetValueWithoutNotify(pref.Value);
                        charField.RegisterValueChangedCallback(i =>
                        {
                            pref.Value = i.newValue;

                            MakeDirty();
                        });

                        horizontal1.Add(charField);

                        conditionBlock.Add(horizontal1);
                    }
                    break;
                default:
                    Debug.LogWarning($"Для типа {item.GetType().Name} нет UI");
                    break;
            }

            extensionContainer.Add(conditionBlock);
        }

        if (Action.Conditions.Count > 0)
        {
            Button removeButton = new Button(() =>
            {
                Action.Conditions.Remove(Action.Conditions.Last());

                UpdateUI();

                MakeDirty();
            })
            {
                text = "Удалить условие"
            };

            extensionContainer.Add(removeButton);
        }
    }

    private string LabelFormater(string typeName)
    {
        Type item = Action.GetType().Assembly.GetTypes()
                        .Where(i => i.BaseType != null && i.BaseType.Name == "ConditionBase" && i.Name == typeName)
                        .FirstOrDefault();

        ConditionBase condition = Activator.CreateInstance(item) as ConditionBase;

        return condition.GetLabel();
    }
}
