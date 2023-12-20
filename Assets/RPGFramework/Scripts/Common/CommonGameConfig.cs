using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "RPGFramework/Config")]
public class CommonGameConfig : ScriptableObject
{
    [Header("Базовое управление")]
    public KeyCode MoveUp = KeyCode.UpArrow;
    public KeyCode MoveDown = KeyCode.DownArrow;
    public KeyCode MoveLeft = KeyCode.LeftArrow;
    public KeyCode MoveRight = KeyCode.RightArrow;

    public KeyCode Run = KeyCode.LeftShift;

    public KeyCode Accept = KeyCode.Z;
    public KeyCode Cancel = KeyCode.X;
    public KeyCode Additional = KeyCode.C;

    [Header("Стартовые переменные")]
    public CustomDictionary<int> IntValues = new CustomDictionary<int>();
    public CustomDictionary<float> FloatValues = new CustomDictionary<float>();
    public CustomDictionary<bool> BoolValues = new CustomDictionary<bool>();
    public CustomDictionary<string> StringValues = new CustomDictionary<string>();
}