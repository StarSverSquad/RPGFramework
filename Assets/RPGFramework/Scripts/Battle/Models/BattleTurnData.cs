public class BattleTurnData
{
    public TurnAction BattleAction { get; set; }

    public RPGCharacter Character { get; private set; }

    public bool IsTarget;
    public bool IsDead;

    public int ReservedConcentration;

    public RPGEnemy EnemyBuffer;
    public RPGCharacter CharacterBuffer;
    public RPGEntity EntityBuffer;

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

    public BattleTurnData(RPGCharacter entity)
    {
        IsDead = false;
        CleanUp();

        Character = entity;

        Character.OnAllStatesChanged += BattleCharacterInfo_OnStatesUpdated;
    }

    private void BattleCharacterInfo_OnStatesUpdated()
    {
        if (IsDead && Character.States.Length > 0)
            Character.RemoveAllStates();
    }

    public void CleanUp()
    {
        BattleAction = TurnAction.None;

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

public enum TurnAction
{
    None, Fight, Act, Item, Flee, Spell
}