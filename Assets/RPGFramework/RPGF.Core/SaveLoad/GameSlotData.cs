using System;
using System.Collections.Generic;
using RPGF;
using RPGF.Domain;
using UnityEngine;

namespace RPGF.Core.SaveLoad
{
    [Serializable]
    public class GameSlotData
    {
        public int Id;

        public string LocationName;

        public CustomDictionary<int> IntValues;
        public CustomDictionary<float> FloatValues;
        public CustomDictionary<bool> BoolValues;
        public CustomDictionary<string> StringValues;

        public List<string> BlockedLocationEvents;

        public CustomDictionary<int> InventoryItems;

        public Vector2 PlayerPosition;
        public ViewDirection PlayerDirection;

        public List<CharacterSaveData> SavedCharacters;

        public GameSlotData()
        {
            IntValues = new CustomDictionary<int>();
            FloatValues = new CustomDictionary<float>();
            BoolValues = new CustomDictionary<bool>();
            StringValues = new CustomDictionary<string>();
            SavedCharacters = new List<CharacterSaveData>();
            InventoryItems = new CustomDictionary<int>();
        }
    }

}