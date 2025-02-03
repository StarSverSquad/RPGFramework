using System;

public class BattleTurnData
{
    public enum TurnAction
    {
        None = -1, Attack, Act, Item, Flee, Ability, Defence
    }

    public TurnAction BattleAction { get; set; }

    public RPGCharacter Character { get; private set; }

    public bool IsTarget;
    public bool IsDead;

    public int ReservedConcentration;

    public RPGEnemy EnemyBuffer;
    public RPGCharacter CharacterBuffer;
    public RPGEntity EntityBuffer;

    #region ACT INFO

    public RPGEnemy.EnemyAct InteractionAct;

    public RPGAbility Ability;

    #endregion

    #region ITEM INFO

    public RPGCollectable Item;
    public bool IsConsumed;

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
        IsConsumed = false;

        Ability = null;
        InteractionAct = RPGEnemy.EnemyAct.NullAct;

        Item = null;

        EnemyBuffer = null;
        CharacterBuffer = null;
        EnemyBuffer = null;

        ReservedConcentration = 0;
    }
}

