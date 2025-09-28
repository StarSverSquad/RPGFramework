using RPGF.Editor.EventSystem;
using RPGF.Editor.EventSystem.Attributes;

[UseActionNodeAttribute("События битвы/Изменить паттерны врага")]
class EnemyBehaviourChangeNode : ActionNodeBase<EnemyBehaviourChangeAction>
{
    public EnemyBehaviourChangeNode(EnemyBehaviourChangeAction action) : base(action)
    {
    }

    public override void UIContructor()
    {
        var typeField = BuildEnumField(
            Action.Type, 
            newVal => Action.Type = newVal,
            val =>
            {
                return val switch
                {
                    EnemyBehaviourChangeAction.ChangeType.DeleteAll => "Удалить все",
                    EnemyBehaviourChangeAction.ChangeType.Delete => "Удалить",
                    EnemyBehaviourChangeAction.ChangeType.Add => "Добавить",
                    _ => "UNKNOWN!"
                };
            },
            updateUI: true
            );

        var enemyTagField = BuildTextField(
            Action.EnemyTag,
            value => Action.EnemyTag = value,
            "Тег врага:"
            );

        AddToExtensionContainer(typeField);
        AddToExtensionContainer(enemyTagField);

        switch (Action.Type)
        {
            case EnemyBehaviourChangeAction.ChangeType.Delete:
                var patternTagField = BuildTextField(
                    Action.PatternTag,
                    value => Action.PatternTag = value,
                    "Тег поведения:"
                    );

                AddToExtensionContainer(patternTagField);
                break;
            case EnemyBehaviourChangeAction.ChangeType.Add:
                var patternField = BuildObjectField(
                    Action.Pattern,
                    value => Action.Pattern = value,
                    "Новое поведение:",
                    allowSceneObjects: false
                    );

                AddToExtensionContainer(patternField);
                break;
        }
    }
}
