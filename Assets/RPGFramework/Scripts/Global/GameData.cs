using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public GameManager Manager;

    public CustomDictionary<int> IntValues;
    public CustomDictionary<float> FloatValues;
    public CustomDictionary<bool> BoolValues;
    public CustomDictionary<string> StringValues;

    public List<string> CachedObjectedEvents = new List<string>();

    public RPGCharacter[] Characters;
    public RPGEntityState[] States;
    public RPGCollectable[] Collectables;

    public int Money = 0;

    public GameData(GameManager manager)
    {
        Manager = manager;

        IntValues = Manager.GameConfig.IntValues;
        FloatValues = Manager.GameConfig.FloatValues;
        BoolValues = Manager.GameConfig.BoolValues;
        StringValues = Manager.GameConfig.StringValues;

        Characters = Resources.LoadAll<RPGCharacter>("Characters");
        States = Resources.LoadAll<RPGEntityState>("EntityStates");
        Collectables = Resources.LoadAll<RPGCollectable>("Items");
    }
}
