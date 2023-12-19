using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[Serializable]
public abstract class TextActionBase
{
    public enum ActionType
    {
        TextReplace, TextAction, All
    }

    private ActionType _actType;
    public ActionType ActType => _actType;

    private Regex _regex;

    public TextWriterBase TextWriter;

    protected string resultText;
    public string ResultText => resultText;

    public TextActionBase(Regex regex, ActionType actType)
    {
        _regex = regex;
        _actType = actType;
    }

    public Coroutine Invoke(MonoBehaviour listner)
    {
        return listner.StartCoroutine(Action());
    }

    public bool MatchRegex(string str) => _regex.IsMatch(str);

    public abstract void CalculateText(string str);

    protected abstract IEnumerator Action();
}
