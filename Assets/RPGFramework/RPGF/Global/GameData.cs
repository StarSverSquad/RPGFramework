using RPGF.Domain;
using RPGF.RPG;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGF
{
    public class GameData : IDisposable
    {
        private readonly GlobalManager _manager;

        public CustomDictionary<int> IntValues;
        public CustomDictionary<float> FloatValues;
        public CustomDictionary<bool> BoolValues;
        public CustomDictionary<string> StringValues;

        public List<string> BlockedLocationEvents;

        public RPGCharacter[] Characters;
        public RPGEntityState[] States;
        public RPGCollectable[] Collectables;
        public RPGAbility[] Abilities;

        public int Money = 0;

        public GameData(GlobalManager manager)
        {
            _manager = manager;

            ApplyDataFromBaseOptions();
        }

        private void ApplyDataFromBaseOptions()
        {
            IntValues = new CustomDictionary<int>();
            FloatValues = new CustomDictionary<float>();
            BoolValues = new CustomDictionary<bool>();
            StringValues = new CustomDictionary<string>();

            BlockedLocationEvents = new List<string>();

            foreach (var data in _manager.BaseOptions.IntValues.data)
                IntValues.Add(data.Key, data.Value);
            foreach (var data in _manager.BaseOptions.FloatValues.data)
                FloatValues.Add(data.Key, data.Value);
            foreach (var data in _manager.BaseOptions.BoolValues.data)
                BoolValues.Add(data.Key, data.Value);
            foreach (var data in _manager.BaseOptions.StringValues.data)
                StringValues.Add(data.Key, data.Value);

            Characters = Resources.LoadAll<RPGCharacter>("Characters");
            States = Resources.LoadAll<RPGEntityState>("EntityStates");
            Collectables = Resources.LoadAll<RPGCollectable>("Items");
            Abilities = Resources.LoadAll<RPGAbility>("Abilities");
        }

        public void Dispose()
        {
            ApplyDataFromBaseOptions();
        }
    }
}