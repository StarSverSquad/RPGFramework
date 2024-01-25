using System;
using UnityEngine.UIElements;

public class ManageVarNode : ActionNodeBase
{
    public ManageVarNode(ManageVarAction action) : base(action)
    {
    }

    public override void UIContructor()
    {
        ManageVarAction mv = action as ManageVarAction;

        EnumField varField = new EnumField("Тип переменной", ManageVarAction.VarType.Bool);

        varField.SetValueWithoutNotify(mv.Var);
        varField.RegisterValueChangedCallback(i =>
        {
            mv.Var = (ManageVarAction.VarType)i.newValue;

            MakeDirty();

            UpdateUI();
        });

        extensionContainer.Add(varField);

        VisualElement hor0 = new VisualElement();
        hor0.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
        hor0.style.justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween);
        hor0.style.marginBottom = 5;

        Label lb0 = new Label("Переменная");
        Label lb1 = new Label("Значение");

        hor0.Add(lb0);
        hor0.Add(lb1);

        extensionContainer.Add(hor0);

        VisualElement hor1 = new VisualElement();
        hor1.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
        hor1.style.justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween);

        TextField nameField = new TextField();

        nameField.SetValueWithoutNotify(mv.VarName);
        nameField.RegisterValueChangedCallback(i =>
        {
            mv.VarName = i.newValue;

            MakeDirty();
        });

        hor1.Add(nameField);

        switch (mv.Var)
        {
            case ManageVarAction.VarType.Bool:
                Label bset = new Label("Set");

                Toggle toggle = new Toggle();

                toggle.SetValueWithoutNotify(mv.BoolBuffer);
                toggle.RegisterValueChangedCallback(i =>
                {
                    mv.BoolBuffer = i.newValue;

                    MakeDirty();
                });

                hor1.Add(bset);
                hor1.Add(toggle);
                break;
            case ManageVarAction.VarType.Int:
                EnumField opField0 = new EnumField(ManageVarAction.OperationType.Set);

                opField0.SetValueWithoutNotify(mv.Operation);
                opField0.RegisterValueChangedCallback(i =>
                {
                    mv.Operation = (ManageVarAction.OperationType)i.newValue;

                    MakeDirty();
                });

                IntegerField intField = new IntegerField();

                intField.SetValueWithoutNotify(mv.IntBuffer);
                intField.RegisterValueChangedCallback(i =>
                {
                    mv.IntBuffer = i.newValue;

                    MakeDirty();
                });

                hor1.Add(opField0);
                hor1.Add(intField);
                break;
            case ManageVarAction.VarType.Float:
                EnumField opField1 = new EnumField(ManageVarAction.OperationType.Set);

                opField1.SetValueWithoutNotify(mv.Operation);
                opField1.RegisterValueChangedCallback(i =>
                {
                    mv.Operation = (ManageVarAction.OperationType)i.newValue;

                    MakeDirty();
                });

                FloatField floatField = new FloatField();

                floatField.SetValueWithoutNotify(mv.FloatBuffer);
                floatField.RegisterValueChangedCallback(i =>
                {
                    mv.FloatBuffer = i.newValue;

                    MakeDirty();
                });

                hor1.Add(opField1);
                hor1.Add(floatField);
                break;
            case ManageVarAction.VarType.String:

                Toggle toggle0 = new Toggle();

                TextField stringField = new TextField();

                stringField.SetValueWithoutNotify(mv.StringBuffer);
                stringField.RegisterValueChangedCallback(i =>
                {
                    mv.StringBuffer = i.newValue;

                    MakeDirty();
                });

                hor1.Add(toggle0);
                hor1.Add(stringField);
                break;
        }

        extensionContainer.Add(hor1);
    }
}