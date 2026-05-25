using RPGF.Core;
using RPGF.Domain;
using RPGF.Domain.DI;
using RPGF.Domain.Interfaces;
using RPGF.RPG;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGF
{
    public class GameData : IDisposable, IManagerInitialize, ISupportDI
    {
        [Inject]
        private readonly BaseOptions _options;

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

        public void Initialize()
        {
            ApplyDataFromBaseOptions();
        }

        private void ApplyDataFromBaseOptions()
        {
            IntValues = new CustomDictionary<int>();
            FloatValues = new CustomDictionary<float>();
            BoolValues = new CustomDictionary<bool>();
            StringValues = new CustomDictionary<string>();

            BlockedLocationEvents = new List<string>();

            foreach (var data in _options.IntValues.data)
                IntValues.Add(data.Key, data.Value);
            foreach (var data in _options.FloatValues.data)
                FloatValues.Add(data.Key, data.Value);
            foreach (var data in _options.BoolValues.data)
                BoolValues.Add(data.Key, data.Value);
            foreach (var data in _options.StringValues.data)
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