using RPGF.Editor.EventSystem;
using RPGF.Editor.EventSystem.Attributes;
using UnityEngine.UIElements;

[UseActionNodeAttribute]
public class ChangeEnemyStatsNode : ActionNodeBase<ChangeEnemyStatsAction>
{
    public ChangeEnemyStatsNode(ChangeEnemyStatsAction Action) : base(Action)
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

        IntegerField damageField = new IntegerField("Damage");

        damageField.SetValueWithoutNotify(Action.newDamage);
        damageField.RegisterValueChangedCallback(i =>
        {
            Action.newDamage = i.newValue;

            MakeDirty();
        });

        IntegerField defenceField = new IntegerField("Defence");

        defenceField.SetValueWithoutNotify(Action.newDefance);
        defenceField.RegisterValueChangedCallback(i =>
        {
            Action.newDefance = i.newValue;

            MakeDirty();
        });

        IntegerField agilityField = new IntegerField("Agility");

        agilityField.SetValueWithoutNotify(Action.newAgility);
        agilityField.RegisterValueChangedCallback(i =>
        {
            Action.newAgility = i.newValue;

            MakeDirty();
        });

        IntegerField luckField = new IntegerField("Luck");

        luckField.SetValueWithoutNotify(Action.newLuck);
        luckField.RegisterValueChangedCallback(i =>
        {
            Action.newLuck = i.newValue;

            MakeDirty();
        });

        extensionContainer.Add(enemyTagField);

        extensionContainer.Add(damageField);
        extensionContainer.Add(defenceField);
        extensionContainer.Add(agilityField);
        extensionContainer.Add(luckField);
    }
}
