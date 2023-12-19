using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "RPG/Ability")]
public class RPGAbility : ScriptableObject
{
    public enum AbilityDirection
    {
        AllTeam, Teammate, AllEnemys, Enemy, Any, All
    }

    public string Name;
    [Multiline()]
    public string Destription;
    public Sprite icon;

    public AbilityDirection Direction;

    public int ManaCost;
    public int ConcentrationCost;

    public int AddHeal;
    public int AddMana;

    public bool WakeupCharacter;

    public List<RPGEntityState> AddStates = new();

    public AttackEffect Effect;

    public GraphEvent StartEvent = null;
    public GraphEvent EndEvent = null;

    public void Invoke(RPGEntity entity)
    {
        entity.Heal += AddHeal;
    }
}
