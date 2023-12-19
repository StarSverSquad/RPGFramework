using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CustomDictionary<T> : IEnumerable<T>
{
    public List<string> keys;
    public List<T> values;

    public CustomDictionary()
    {
        keys = new List<string>();
        values = new List<T>();
    }

    public T this[string key]
    {
        get
        {
            int index = keys.IndexOf(key);

            return values[index];
        }

        set
        {
            int index = keys.IndexOf(key);

            values[index] = value;
        }
    } 

    public void Add(string key, T value)
    {
        if (!keys.Contains(key))
        {
            keys.Add(key);
            values.Add(value);
        }
        else
            Debug.LogError($"Ключ \"{key}\" уже существует");
    }

    public void Remove(string key)
    {
        if (keys.Contains(key))
        {
            int index = keys.IndexOf(key);

            keys.RemoveAt(index);
            values.RemoveAt(index);
        }
        else
            Debug.LogError($"Ключ \"{key}\" не существует");
    }

    public bool HaveKey(string key)
    {
        foreach (var item in keys)
        {
            if (item == key)
                return true;
        }
        
        return false;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
