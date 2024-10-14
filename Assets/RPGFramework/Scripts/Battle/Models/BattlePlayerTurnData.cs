public class BattlePlayerTurnData
{
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
}