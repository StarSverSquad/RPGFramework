using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityState", menuName = "RPG/EntityState")]
public class RPGEntityState : ScriptableObject
{
    public GraphEvent Event;

    public string Name;
    public Color Color;

    public Sprite Icon;

    [Tooltip("��������� ����������� ����!")]
    public bool SkipTurn;

    public int AddHeal;
    public int AddMana;

    public int AddDamage;
    public int AddDefence;
    public int AddAgility;

    public int TurnCount;
}
