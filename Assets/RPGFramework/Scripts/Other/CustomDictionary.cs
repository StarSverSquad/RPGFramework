using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class CustomDictionary<T> : IEnumerable<T>
{
    [Serializable]
    public class DictionaryItem
    {
        public string Key;
        public T Value;

        public DictionaryItem()
        {
            Key = string.Empty;
            Value = default;
        }
        public DictionaryItem(string key, T value)
        {
            Key = key;
            Value = value;
        }
    }

    public List<DictionaryItem> data;

    public CustomDictionary()
    {
        data = new();
    }

    public T this[string key]
    {
        get => Get(key);

        set
        {
            if (!HaveKey(key))
                Add(key, value);
            else
                Set(key, value);
        }
    } 

    public void Add(string key, T value)
    {
        if (!HaveKey(key))
            data.Add(new DictionaryItem(key, value));
        else
            Debug.LogError($"���� \"{key}\" ��� ����������");
    }

    public void Remove(string key)
    {
        if (HaveKey(key))
            data.Remove(data.Where(i => i.Key == key).FirstOrDefault());
        else
            Debug.LogError($"���� \"{key}\" �� ����������");
    }

    public bool HaveKey(string key)
    {
        return data.Where(i => i.Key == key).Count() > 0;
    }

    public T Get(string key)
    {
        return data.FirstOrDefault(i => i.Key == key).Value;
    }
    public void Set(string key, T value)
    {
        data.FirstOrDefault(i => i.Key == key).Value = value;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return data.Select(i => i.Value).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
