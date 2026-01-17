using RPGF.Core.TextWriter.Abstrations;
using RPGF.Domain.TP;
using RPGF.Domain.TP.Abstractions;
using System.Collections;
using UnityEngine;

namespace RPGF.Core.TextWriter.Actions
{
    [UseTextAction(@"^\\(\.|:|\|)$", TextActionType.Single)]
    public class WaitTextAction : TextWriterActionBase
    {
        public float ParseText(string str)
        {
            return str[1] switch
            {
                '.' => .25f,
                ':' => .5f,
                '|' => 1f,
                _ => 0f,
            };
        }

        public override IEnumerator Action(TextActionParams @params)
        {
            yield return new WaitForSeconds(ParseText(@params.Tag));
        }
    }
}