using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleInfo", menuName = "RPG/BattleInfo")]
public class RPGBattleInfo : ScriptableObject
{
    [Header("����� ���������")]
    public RPGEnemySquad enemySquad;
    [Tooltip("���� �������� ������?")]
    public bool EnemyStart = false;
    [Tooltip("����� �� ���������?")]
    public bool CanLose = true;
    [Tooltip("����� �� �������?")]
    public bool CanFlee = false;
    [Header("��������� �����")]
    public AudioClip BattleMusic;
    public float MusicVolume;
    [Header("���������")]
    public GameObject Background;
}
