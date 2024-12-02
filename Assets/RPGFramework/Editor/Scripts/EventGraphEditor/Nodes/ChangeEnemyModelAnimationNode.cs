using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[UseActionNode]
public class ChangeEnemyModelAnimationNode : ActionNodeWrapper<ChangeEnemyModelAnimationAction>
{
    public ChangeEnemyModelAnimationNode(ChangeEnemyModelAnimationAction Action) : base(Action)
    {
    }

    public override void UIContructor()
    {
        TextField enemyTagField = new TextField("Тег врага");

        enemyTagField.SetValueWithoutNotify(Action.EnemyTag);
        enemyTagField.RegisterValueChangedCallback(i =>
        {
            Action.EnemyTag = i.newValue;

            MakeDirty();
        });

        TextField animatorTagField = new TextField("Тег аниматора");

        animatorTagField.SetValueWithoutNotify(Action.AnimatorTag);
        animatorTagField.RegisterValueChangedCallback(i =>
        {
            Action.AnimatorTag = i.newValue;

            MakeDirty();
        });

        TextField triggerField = new TextField("Триггер");

        triggerField.SetValueWithoutNotify(Action.Trigger);
        triggerField.RegisterValueChangedCallback(i =>
        {
            Action.Trigger = i.newValue;

            MakeDirty();
        });

        extensionContainer.Add(enemyTagField);
        extensionContainer.Add(animatorTagField);
        extensionContainer.Add(triggerField);
    }
}
