using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class InsertLocaleAction : TextActionBase
{
    public InsertLocaleAction() : base(new Regex(@"^%(\w|_)+$"), ActionType.TextReplace)
    {
        
    }

    public override string GetText(string str)
    {
        string tag = str.Remove(0, 1);

        return GameManager.ILocalization.GetLocale(tag);
    }

    protected override IEnumerator Action()
    {
        yield break;
    }
}
