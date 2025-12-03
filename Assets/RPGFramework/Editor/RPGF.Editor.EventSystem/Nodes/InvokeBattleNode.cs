using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor.UIElements;
using RPGF.RPG;
using RPGF.Editor.EventSystem.Attributes;
using RPGF.Actions;

namespace RPGF.Editor.EventSystem.Nodes
{
    [UseActionNode("Битва", contextualMenuPath: "Битва/Битва!")]
    public class InvokeBattleNode : ActionNodeBase<InvokeBattleAction>
    {
        public InvokeBattleNode(InvokeBattleAction Action) : base(Action)
        {

        }

        public override void PortContructor()
        {
            CreateInputPort("Вход", "Input");

            var winNext = action.GetNext(InvokeBattleAction.WinNextTag);

            CreateOutputPort(winNext.Name, winNext.Tag, InvokeBattleAction.WinNextTag);

            if (Action.battle != null)
            {
                if (Action.battle.CanFlee && Action.branchFlee)
                {
                    var fleeNext = action.GetNext(InvokeBattleAction.FleeNextTag);

                    CreateOutputPort(fleeNext.Name, fleeNext.Tag, Color.yellow, InvokeBattleAction.FleeNextTag);
                }

                if (Action.battle.CanLose)
                {
                    var loseNext = action.GetNext(InvokeBattleAction.LoseNextTag);

                    CreateOutputPort(loseNext.Name, loseNext.Tag, Color.red, InvokeBattleAction.LoseNextTag);
                }
            }
        }

        public override void UIContructor()
        {
            ObjectField battleField = new("Битва")
            {
                objectType = typeof(RPGBattleInfo),
                allowSceneObjects = false
            };

            battleField.SetValueWithoutNotify(Action.battle);
            battleField.RegisterValueChangedCallback(val =>
            {
                Action.battle = (RPGBattleInfo)val.newValue;

                UpdatePorts();

                MakeDirty();
            });

            extensionContainer.Add(battleField);

            Toggle fleeToggle = new("Выделить побег?");

            fleeToggle.SetValueWithoutNotify(Action.branchFlee);
            fleeToggle.RegisterValueChangedCallback(val =>
            {
                Action.branchFlee = val.newValue;

                UpdatePorts();

                MakeDirty();
            });

            extensionContainer.Add(fleeToggle);
        }
    }
}