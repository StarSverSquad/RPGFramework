using RPGF.Actions;
using RPGF.Editor.EventSystem.Attributes;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace RPGF.Editor.EventSystem.Nodes
{
    [UseActionNode("Выбор", contextualMenuPath: "Ветвление/Выбор")]
    public class ChoiceNode : ActionNodeBase<ChoiceAction>
    {
        private List<Port> outports;
        private List<TextField> choiceFields;

        public ChoiceNode(ChoiceAction Action) : base(Action)
        {
            outports = new List<Port>();

            for (int i = 0; i < Action.Choices.Count; i++)
            {
                Port port = CreateOutputPort($"Выбор {i + 1}:", $"Choice-{i}");

                outports.Add(port);
                outputContainer.Add(port);
            }
        }

        private void UpdateValue(TextField tf)
        {
            int index = choiceFields.IndexOf(tf);

            Action.Choices[index] = tf.value;
        }

        public override void PortContructor()
        {
            CreateInputPort("Вход", "Input");

            outputContainer.Add(new VisualElement());
        }

        public override void UIContructor()
        {
            choiceFields = new List<TextField>();

            EnumField posField = new("Позиция", ChoiceBoxManager.Position.Bottom);

            posField.SetValueWithoutNotify(Action.Position);
            posField.RegisterValueChangedCallback(i =>
            {
                Action.Position = (ChoiceBoxManager.Position)i.newValue;

                MakeDirty();

                UpdateUI();
            });

            extensionContainer.Add(posField);

            if (Action.Position == ChoiceBoxManager.Position.Custom)
            {
                Label cposLabel = new("Координаты в 1920x1080");

                Vector2Field cposField = new();
                cposField.SetValueWithoutNotify(Action.CustomPosition);
                cposField.RegisterValueChangedCallback(i =>
                {
                    Action.CustomPosition = i.newValue;

                    MakeDirty();
                });

                extensionContainer.Add(cposLabel);
                extensionContainer.Add(cposField);
            }

            VisualElement horizontal0 = new();
            horizontal0.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);

            Button addbutton = new()
            {
                text = "Add"
            };
            addbutton.clicked += () =>
            {
                Action.Choices.Add(string.Empty);
                Action.AddNext($"Choice-{outports.Count - 1}");

                Port port = CreateOutputPort($"Выбор {outports.Count}", $"Choice-{outports.Count - 1}");

                outports.Add(port);

                outputContainer.Add(port);

                UpdateUI();
                MakeDirty();
            };

            Button removebutton = new()
            {
                text = "Remove",
            };
            removebutton.clicked += () =>
            {
                outputContainer.Remove(outports.Last());

                outports.Remove(outports.Last());

                Action.Choices.Remove(Action.Choices.Last());

                UpdateUI();
                MakeDirty();
            };

            for (int i = 0; i < Action.Choices.Count; i++)
            {
                TextField field = new ($"Choice {i}:");

                field.SetValueWithoutNotify(Action.Choices[i]);
                field.RegisterValueChangedCallback(item =>
                {
                    UpdateValue(field);

                    MakeDirty();
                });

                choiceFields.Add(field);
                extensionContainer.Add(field);
            }

            if (outports.Count > 0)
            {
                horizontal0.Add(removebutton);
            }

            horizontal0.Add(addbutton);

            extensionContainer.Add(horizontal0);
        }
    }
}