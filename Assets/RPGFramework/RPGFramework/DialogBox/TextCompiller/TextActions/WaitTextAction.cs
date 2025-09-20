using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class WaitTextAction : TextActionBase
{
    float waitTime;

    public WaitTextAction() : base(new Regex(@"^\\(\.|:|\|)$"), ActionType.TextAction)
    {
        waitTime = 0;
    }

    public override void ParseText(string str)
    {
        switch (str[1])
        {
            case '.':
                waitTime = 0.25f;
                break;
            case ':':
                waitTime = 0.5f;
                break;
            case '|':
                waitTime = 1f;
                break;
        }
    }

    protected override IEnumerator Action()
    {
        yield return new WaitForSeconds(waitTime);
    }
}
