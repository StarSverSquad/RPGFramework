using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class ChoiceNode : ActionNodeBase
{
    private List<Port> outports;
    private List<TextField> choiceFields;

    public ChoiceNode(ChoiceAction action) : base(action)
    {
        outports = new List<Port>();

        for (int i = 0; i < action.Choices.Count; i++)
        {
            Port port = CreateOutputPort($"Choice {outports.Count}");

            outports.Add(port);

            outputContainer.Add(port);
        }
    }

    private void UpdateValue(TextField tf)
    {
        int index = choiceFields.IndexOf(tf);
     
        ChoiceAction ca = action as ChoiceAction;

        ca.Choices[index] = tf.value;
    }

    public override void PortContructor()
    {
        CreateInputPort("Input");

        outputContainer.Add(new VisualElement());
    }

    public override void UIContructor()
    {
        ChoiceAction ca = action as ChoiceAction;

        choiceFields = new List<TextField>();

        EnumField posField = new EnumField("Позиция", ChoiceBoxManager.Position.Bottom);

        posField.SetValueWithoutNotify(ca.Position);
        posField.RegisterValueChangedCallback(i =>
        {
            ca.Position = (ChoiceBoxManager.Position)i.newValue;

            MakeDirty();

            UpdateUI();
        });

        extensionContainer.Add(posField);

        if (ca.Position == ChoiceBoxManager.Position.Custom)
        {
            Label cposLabel = new Label("Координаты в 1920x1080");
            Vector2Field cposField = new Vector2Field();

            cposField.SetValueWithoutNotify(ca.CustomPosition);
            cposField.RegisterValueChangedCallback(i =>
            {
                ca.CustomPosition = i.newValue;

                MakeDirty();
            });

            extensionContainer.Add(cposLabel);
            extensionContainer.Add(cposField);
        }

        VisualElement horizontal0 = new VisualElement();
        horizontal0.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);

        Button addbutton = new Button()
        {
            text = "Add"
        };
        addbutton.clicked += () =>
        {
            Port port = CreateOutputPort($"Choice {outports.Count}");

            outports.Add(port);

            outputContainer.Add(port);

            ca.Choices.Add(string.Empty);

            UpdateUI();

            MakeDirty();
        };

        Button removebutton = new Button()
        {
            text = "Remove",
        };
        removebutton.clicked += () =>
        {
            outputContainer.Remove(outports.Last());

            outports.Remove(outports.Last());

            ca.Choices.Remove(ca.Choices.Last());

            UpdateUI();

            MakeDirty();
        };

        for (int i = 0; i < ca.Choices.Count; i++)
        {
            TextField field = new TextField($"Choice {i}:");

            

            field.SetValueWithoutNotify(ca.Choices[i]);
            field.RegisterValueChangedCallback(item =>
            {
                UpdateValue(field);

                MakeDirty();
            });

            choiceFields.Add(field);

            extensionContainer.Add(field);
        }

        if (outports.Count > 0)
            horizontal0.Add(removebutton);

        horizontal0.Add(addbutton);

        extensionContainer.Add(horizontal0);
    }
}