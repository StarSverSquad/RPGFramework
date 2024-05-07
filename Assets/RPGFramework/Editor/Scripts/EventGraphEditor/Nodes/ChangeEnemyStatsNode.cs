using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ChangeEnemyStatsNode : ActionNodeBase
{
    public ChangeEnemyStatsNode(ChangeEnemyStatsAction action) : base(action)
    {
    }

    public override void UIContructor()
    {
        ChangeEnemyStatsAction se = action as ChangeEnemyStatsAction;

        TextField enemyTagField = new TextField("Тег врага");

        enemyTagField.SetValueWithoutNotify(se.EnemyTag);
        enemyTagField.RegisterValueChangedCallback(i =>
        {
            se.EnemyTag = i.newValue;

            MakeDirty();
        });

        IntegerField damageField = new IntegerField("Damage");

        damageField.SetValueWithoutNotify(se.newDamage);
        damageField.RegisterValueChangedCallback(i =>
        {
            se.newDamage = i.newValue;

            MakeDirty();
        });

        IntegerField defenceField = new IntegerField("Defence");

        defenceField.SetValueWithoutNotify(se.newDefance);
        defenceField.RegisterValueChangedCallback(i =>
        {
            se.newDefance = i.newValue;

            MakeDirty();
        });

        IntegerField agilityField = new IntegerField("Agility");

        agilityField.SetValueWithoutNotify(se.newAgility);
        agilityField.RegisterValueChangedCallback(i =>
        {
            se.newAgility = i.newValue;

            MakeDirty();
        });

        IntegerField luckField = new IntegerField("Luck");

        luckField.SetValueWithoutNotify(se.newLuck);
        luckField.RegisterValueChangedCallback(i =>
        {
            se.newLuck = i.newValue;

            MakeDirty();
        });

        extensionContainer.Add(enemyTagField);

        extensionContainer.Add(damageField);
        extensionContainer.Add(defenceField);
        extensionContainer.Add(agilityField);
        extensionContainer.Add(luckField);
    }
}
