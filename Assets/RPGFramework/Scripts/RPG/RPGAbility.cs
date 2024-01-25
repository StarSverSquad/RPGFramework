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

    public bool ForDeath;
    public bool ForAlive;

    public int Damage;

    public bool WakeupCharacter;

    public GraphEvent StartEvent;
    public GraphEvent EndEvent;

    public VisualAttackEffect VisualEffect;

    [HideInInspector]
    [SerializeReference]
    public List<EffectBase> Effects = new List<EffectBase>();
}
