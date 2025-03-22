using System;
using UnityEngine.UIElements;

[UseActionNode]
public class ManageVarNode : ActionNodeWrapper<ManageVarAction>
{
    public ManageVarNode(ManageVarAction Action) : base(Action)
    {
    }

    public override void UIContructor()
    {
        EnumField varField = new EnumField("Тип переменной", ManageVarAction.VarType.Bool);

        varField.SetValueWithoutNotify(Action.Var);
        varField.RegisterValueChangedCallback(i =>
        {
            Action.Var = (ManageVarAction.VarType)i.newValue;

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

        nameField.SetValueWithoutNotify(Action.VarName);
        nameField.RegisterValueChangedCallback(i =>
        {
            Action.VarName = i.newValue;

            MakeDirty();
        });

        hor1.Add(nameField);

        switch (Action.Var)
        {
            case ManageVarAction.VarType.Bool:
                Label bset = new Label("Set");

                Toggle toggle = new Toggle();

                toggle.SetValueWithoutNotify(Action.BoolBuffer);
                toggle.RegisterValueChangedCallback(i =>
                {
                    Action.BoolBuffer = i.newValue;

                    MakeDirty();
                });

                hor1.Add(bset);
                hor1.Add(toggle);
                break;
            case ManageVarAction.VarType.Int:
                EnumField opField0 = new EnumField(ManageVarAction.OperationType.Set);

                opField0.SetValueWithoutNotify(Action.Operation);
                opField0.RegisterValueChangedCallback(i =>
                {
                    Action.Operation = (ManageVarAction.OperationType)i.newValue;

                    MakeDirty();
                });

                IntegerField intField = new IntegerField();

                intField.SetValueWithoutNotify(Action.IntBuffer);
                intField.RegisterValueChangedCallback(i =>
                {
                    Action.IntBuffer = i.newValue;

                    MakeDirty();
                });

                hor1.Add(opField0);
                hor1.Add(intField);
                break;
            case ManageVarAction.VarType.Float:
                EnumField opField1 = new EnumField(ManageVarAction.OperationType.Set);

                opField1.SetValueWithoutNotify(Action.Operation);
                opField1.RegisterValueChangedCallback(i =>
                {
                    Action.Operation = (ManageVarAction.OperationType)i.newValue;

                    MakeDirty();
                });

                FloatField floatField = new FloatField();

                floatField.SetValueWithoutNotify(Action.FloatBuffer);
                floatField.RegisterValueChangedCallback(i =>
                {
                    Action.FloatBuffer = i.newValue;

                    MakeDirty();
                });

                hor1.Add(opField1);
                hor1.Add(floatField);
                break;
            case ManageVarAction.VarType.String:

                Label label = new Label("Set");

                TextField stringField = new TextField();

                stringField.SetValueWithoutNotify(Action.StringBuffer);
                stringField.RegisterValueChangedCallback(i =>
                {
                    Action.StringBuffer = i.newValue;

                    MakeDirty();
                });

                hor1.Add(label);
                hor1.Add(stringField);
                break;
            case ManageVarAction.VarType.FastSave:

                EnumField opField2 = new EnumField(ManageVarAction.OperationType.Set);

                opField2.SetValueWithoutNotify(Action.Operation);
                opField2.RegisterValueChangedCallback(i =>
                {
                    Action.Operation = (ManageVarAction.OperationType)i.newValue;

                    MakeDirty();
                });

                IntegerField prefField = new IntegerField();

                prefField.SetValueWithoutNotify(Action.IntBuffer);
                prefField.RegisterValueChangedCallback(i =>
                {
                    Action.IntBuffer = i.newValue;

                    MakeDirty();
                });

                hor1.Add(opField2);

                hor1.Add(prefField);
                break;
        }

        extensionContainer.Add(hor1);
    }
}