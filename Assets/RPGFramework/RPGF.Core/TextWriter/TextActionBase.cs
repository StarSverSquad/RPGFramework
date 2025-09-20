using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

[Serializable]
public abstract class TextActionBase
{
    public enum ActionType
    {
        TextReplace, TextAction
    }

    public TextWriterBase TextWriter;

    public ActionType Type { get; private set; }
    public Regex Regex { get; private set; }

    public TextActionBase(Regex regex, ActionType actType)
    {
        this.Regex = regex;
        this.Type = actType;
    }

    public Coroutine Invoke(MonoBehaviour listner)
    {
        return listner.StartCoroutine(Action());
    }

    public bool MatchRegex(string str) => Regex.IsMatch(str);

    /// <summary>
    /// Нужен для обработки текста внутри тега.
    /// Используеться для любой теговой обработки.
    /// </summary>
    public virtual void ParseText(string str) { }

    /// <summary>
    /// Нужен для обработки текста внутри тега и возращения нового.
    /// Используется для подмены текста.
    /// </summary>
    public virtual string GetText(string str) => "";

    protected virtual IEnumerator Action() { yield break; } 
}
