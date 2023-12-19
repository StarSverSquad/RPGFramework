public class BattleCharacterInfo : BattleEntityInfo
{
    public BattleCharacterAction BattleAction { get; set; }

    public RPGCharacter Character => Entity as RPGCharacter;

    public bool IsTarget;
    public bool IsDead;

    public BattleEnemyInfo EnemyBuffer;
    public BattleCharacterInfo CharacterBuffer;
    public BattleEntityInfo EntityBuffer;

    #region ACT INFO

    public bool IsAbility;

    public RPGEnemy.EnemyAct InteractionAct;

    public bool AbilityToAll;
    public RPGAbility Ability;

    #endregion

    #region ITEM INFO

    public RPGCollectable Item;
    public bool IsConsumed;

    #endregion

    #region DEFENCE INFO

    public bool IsDefence;
    public bool IsFlee;

    #endregion

    public BattleCharacterInfo(RPGCharacter entity) : base(entity)
    {
        IsDead = false;
        CleanUp();
    }

    public void CleanUp()
    {
        BattleAction = BattleCharacterAction.None;

        IsTarget = false;
        IsAbility = false;
        IsConsumed = false;
        AbilityToAll = false;
        IsDefence = false;
       
        Ability = null;
        InteractionAct = new RPGEnemy.EnemyAct();

        Item = null;

        EnemyBuffer = null;
        CharacterBuffer = null;
        EnemyBuffer = null;
    }
}

public enum BattleCharacterAction
{
    None, Fight, Act, Spell, Item, Defence
}