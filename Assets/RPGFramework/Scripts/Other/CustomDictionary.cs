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
            this.Key = key;
            Value = value;
        }
    }

    public List<DictionaryItem> data;

    public CustomDictionary()
    {
        data = new List<DictionaryItem>();
    }

    public T this[string key]
    {
        get => data.Where(i => i.Key == key).FirstOrDefault().Value;

        set => data.Where(i => i.Key == key).FirstOrDefault().Value = value;
    } 

    public void Add(string key, T value)
    {
        if (!HaveKey(key))
            data.Add(new DictionaryItem(key, value));
        else
            Debug.LogError($"Ключ \"{key}\" уже существует");
    }

    public void Remove(string key)
    {
        if (HaveKey(key))
            data.Remove(data.Where(i => i.Key == key).FirstOrDefault());
        else
            Debug.LogError($"Ключ \"{key}\" не существует");
    }

    public bool HaveKey(string key)
    {
        return data.Where(i => i.Key == key).Count() > 0;
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
