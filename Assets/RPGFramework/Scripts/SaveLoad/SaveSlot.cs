using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveSlot
{
    public int Id;

    public int Money;

    public CustomDictionary<int> IntValues;
    public CustomDictionary<float> FloatValues;
    public CustomDictionary<bool> BoolValues;
    public CustomDictionary<string> StringValues;

    public List<CharacterSaveInfo> Characters;

    public SaveSlot()
    {
        IntValues = new CustomDictionary<int>();
        FloatValues = new CustomDictionary<float>();
        BoolValues = new CustomDictionary<bool>();
        StringValues = new CustomDictionary<string>();

        Characters = new List<CharacterSaveInfo>();
    }
}
