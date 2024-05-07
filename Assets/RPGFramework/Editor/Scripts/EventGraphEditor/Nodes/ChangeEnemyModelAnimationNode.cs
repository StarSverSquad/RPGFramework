using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ChangeEnemyModelAnimationNode : ActionNodeBase
{
    public ChangeEnemyModelAnimationNode(ChangeEnemyModelAnimationAction action) : base(action)
    {
    }

    public override void UIContructor()
    {
        ChangeEnemyModelAnimationAction se = action as ChangeEnemyModelAnimationAction;

        TextField enemyTagField = new TextField("��� �����");

        enemyTagField.SetValueWithoutNotify(se.EnemyTag);
        enemyTagField.RegisterValueChangedCallback(i =>
        {
            se.EnemyTag = i.newValue;

            MakeDirty();
        });

        TextField animatorTagField = new TextField("��� ���������");

        animatorTagField.SetValueWithoutNotify(se.AnimatorTag);
        animatorTagField.RegisterValueChangedCallback(i =>
        {
            se.AnimatorTag = i.newValue;

            MakeDirty();
        });

        TextField triggerField = new TextField("�������");

        triggerField.SetValueWithoutNotify(se.Trigger);
        triggerField.RegisterValueChangedCallback(i =>
        {
            se.Trigger = i.newValue;

            MakeDirty();
        });

        extensionContainer.Add(enemyTagField);
        extensionContainer.Add(animatorTagField);
        extensionContainer.Add(triggerField);
    }
}
