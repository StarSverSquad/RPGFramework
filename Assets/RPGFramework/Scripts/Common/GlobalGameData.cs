using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameData : MonoBehaviour
{
    public CustomDictionary<int> IntValues = new CustomDictionary<int>();
    public CustomDictionary<float> FloatValues = new CustomDictionary<float>();
    public CustomDictionary<bool> BoolValues = new CustomDictionary<bool>();
    public CustomDictionary<string> StringValues = new CustomDictionary<string>();

    public List<string> CachedObjectedEvents = new List<string>();

    public int Money = 0;
}
