using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : IDisposable
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
    public RPGAbility[] Abilities;

    public int Money = 0;

    public GameData(GameManager manager)
    {
        Manager = manager;

        UpdateData();
    }

    private void UpdateData()
    {
        IntValues = new CustomDictionary<int>();
        FloatValues = new CustomDictionary<float>();
        BoolValues = new CustomDictionary<bool>();
        StringValues = new CustomDictionary<string>();

        foreach (var data in Manager.BaseOptions.IntValues.data)
            IntValues.Add(data.Key, data.Value);
        foreach (var data in Manager.BaseOptions.FloatValues.data)
            FloatValues.Add(data.Key, data.Value);
        foreach (var data in Manager.BaseOptions.BoolValues.data)
            BoolValues.Add(data.Key, data.Value);
        foreach (var data in Manager.BaseOptions.StringValues.data)
            StringValues.Add(data.Key, data.Value);

        Characters = Resources.LoadAll<RPGCharacter>("Characters");
        States = Resources.LoadAll<RPGEntityState>("EntityStates");
        Collectables = Resources.LoadAll<RPGCollectable>("Items");
        Abilities = Resources.LoadAll<RPGAbility>("Abilities");
    }

    public void Dispose()
    {
        UpdateData();
    }
}
