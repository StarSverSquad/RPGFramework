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

    public string Tag;
    public string Name;
    [Multiline()]
    public string Destription;
    public Sprite icon;
    [Space]
    public AbilityDirection Direction;
    [Space]
    public int ManaCost;
    public int ConcentrationCost;
    [Space]
    public bool ForDeath;
    public bool ForAlive;

    [Tooltip("Если положительный значит лечит, если отрцатльный значит наносит урон. Учитывает все внешние факторы.")]
    public int Formula;
    [Space]
    public bool WakeupCharacter;
    [Space]
    public GraphEvent StartEvent;
    public GraphEvent EndEvent;
    [Space]
    public VisualAttackEffect VisualEffect;
    [Space]
    public MinigameBase Minigame;

    [HideInInspector]
    [SerializeReference]
    public List<EffectBase> Effects = new List<EffectBase>();
}
