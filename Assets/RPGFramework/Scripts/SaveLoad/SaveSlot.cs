using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO.Pipes;
using System.Linq;
using UnityEngine;

[Serializable]
public class SaveSlot
{
    public int Id;

    public string LocationName;

    public CustomDictionary<int> IntValues;
    public CustomDictionary<float> FloatValues;
    public CustomDictionary<bool> BoolValues;
    public CustomDictionary<string> StringValues;
    public CustomDictionary<int> InventoryItems;
    public Vector2 PlayerPosition;

    public List<CharacterSaveInfo> SavedCharacters;

    public SaveSlot()
    {
        IntValues = new CustomDictionary<int>();
        FloatValues = new CustomDictionary<float>();
        BoolValues = new CustomDictionary<bool>();
        StringValues = new CustomDictionary<string>();
        SavedCharacters = new List<CharacterSaveInfo>();
        InventoryItems = new CustomDictionary<int>();
    }

}
