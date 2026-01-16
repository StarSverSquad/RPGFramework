using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

namespace RPGF.Core.TextWriter.Actions
{
    [UseTextWriterAction(@"^\\(\.|:|\|)$", TextActionType.Instance)]
    public class WaitTextAction : TextActionBase
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

        protected override IEnumerator Action(TextActionParams @params)
        {
            yield return new WaitForSeconds(ParseText(@params.Tag));
        }
    }
}