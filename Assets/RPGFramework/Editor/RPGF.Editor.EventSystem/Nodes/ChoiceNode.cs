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
        }

        private void UpdateValue(TextField tf)
        {
            int index = choiceFields.IndexOf(tf);

            Action.Choices[index] = tf.value;
        }

        public override void PortContructor()
        {
            CreateInputPort("Вход", "Input");

            outports = new List<Port>();
            for (int i = 0; i < Action.Choices.Count; i++)
            {
                Port port = CreateOutputPort($"Выбор {i + 1}:", $"Choice-{i}", $"Choice-{i}");

                outports.Add(port);
                outputContainer.Add(port);
            }

            if (Action.Choices.Count == 0)
            {
                outputContainer.Add(new VisualElement());
            }
        }

        public override void UIContructor()
        {
            choiceFields = new List<TextField>();

            var blockToggle = BuildToggle(Action.canCancel, (newValue) =>
            {
                Action.canCancel = newValue;

                UpdateUI();
                MakeDirty();
            }, label: "Можно отменить?");

            extensionContainer.Add(blockToggle);

            if (Action.canCancel)
            {
                var fallbackPopup = BuildPopupField(Action.fallbackIndex, Action.Choices, (newValue) =>
                {
                    Action.fallbackIndex = Action.Choices.IndexOf(newValue);

                    MakeDirty();
                }, label: "Автовыбор при отмене");

                extensionContainer.Add(fallbackPopup);
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
                Action.AddNext($"Choice-{Action.Choices.Count - 1}");

                UpdatePorts();

                UpdateUI();
                MakeDirty();
            };

            Button removebutton = new()
            {
                text = "Remove",
            };
            removebutton.clicked += () =>
            {
                Action.Choices.Remove(Action.Choices.Last());
                Action.Nexts.Remove(Action.Nexts.Last());

                UpdatePorts();

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