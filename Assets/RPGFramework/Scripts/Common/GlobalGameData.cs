using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameData : MonoBehaviour, IManagerInitialize
{
    public CustomDictionary<int> IntValues = new CustomDictionary<int>();
    public CustomDictionary<float> FloatValues = new CustomDictionary<float>();
    public CustomDictionary<bool> BoolValues = new CustomDictionary<bool>();
    public CustomDictionary<string> StringValues = new CustomDictionary<string>();

    public List<string> CachedObjectedEvents = new List<string>();

    public int Money = 0;

    public void Initialize()
    {
        IntValues = GameManager.Instance.CommonConfig.IntValues;
        FloatValues = GameManager.Instance.CommonConfig.FloatValues;
        BoolValues = GameManager.Instance.CommonConfig.BoolValues;
        StringValues = GameManager.Instance.CommonConfig.StringValues;
    }
}
