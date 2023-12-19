using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class PauseTextAction : TextActionBase
{
    public PauseTextAction() : base(new Regex(@"!"), ActionType.TextAction)
    {
        
    }

    public override void CalculateText(string str)
    {
        
    }

    protected override IEnumerator Action()
    {
        TextWriter.PauseWrite();

        yield break;
    }
}
