public class BattleCharacterInfo : BattleEntityInfo
{
    public BattleCharacterAction BattleAction { get; set; }

    public RPGCharacter Character => Entity as RPGCharacter;

    public bool IsTarget;
    public bool IsDead;

    public int ReservedConcentration;

    public BattleEnemyInfo EnemyBuffer;
    public BattleCharacterInfo CharacterBuffer;
    public BattleEntityInfo EntityBuffer;

    #region ACT INFO

    public bool IsAbility;

    public RPGEnemy.EnemyAct InteractionAct;

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

        Character.OnAllStatesChanged += BattleCharacterInfo_OnStatesUpdated;
    }

    private void BattleCharacterInfo_OnStatesUpdated()
    {
        if (IsDead && Character.States.Length > 0)
            Character.RemoveAllStates();
    }

    public void CleanUp()
    {
        BattleAction = BattleCharacterAction.None;

        IsTarget = false;
        IsAbility = false;
        IsConsumed = false;
        IsDefence = false;
       
        Ability = null;
        InteractionAct = RPGEnemy.EnemyAct.NullAct;

        Item = null;

        EnemyBuffer = null;
        CharacterBuffer = null;
        EnemyBuffer = null;

        ReservedConcentration = 0;
    }
}

public enum BattleCharacterAction
{
    None, Fight, Act, Item, Defence
}