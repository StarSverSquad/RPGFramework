[UseActionNode("События битвы/Изменить паттерны врага")]
class EnemyPatternChangeNode : ActionNodeWrapper<EnemyPatternChangeAction>
{
    public EnemyPatternChangeNode(EnemyPatternChangeAction action) : base(action)
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
                    EnemyPatternChangeAction.ChangeType.DeleteAll => "Удалить все",
                    EnemyPatternChangeAction.ChangeType.Delete => "Удалить",
                    EnemyPatternChangeAction.ChangeType.Add => "Добавить",
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
            case EnemyPatternChangeAction.ChangeType.Delete:
                var patternTagField = BuildTextField(
                    Action.PatternTag,
                    value => Action.PatternTag = value,
                    "Тег паттерна:"
                    );

                AddToExtensionContainer(patternTagField);
                break;
            case EnemyPatternChangeAction.ChangeType.Add:
                var patternField = BuildObjectField(
                    Action.Pattern,
                    value => Action.Pattern = value,
                    "Паттерн:",
                    allowSceneObjects: false
                    );

                AddToExtensionContainer(patternField);
                break;
        }
    }
}
