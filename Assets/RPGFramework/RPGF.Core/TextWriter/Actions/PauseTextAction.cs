using System.Collections;
using System.Text.RegularExpressions;

namespace RPGF.Core.TextWriter.Actions
{
    [UseTextWriterAction(@"^!$", TextActionType.Instance)]
    public class PauseTextAction : TextActionBase
    {
        protected override IEnumerator Action(TextActionParams @params)
        {
            _textWriter.PauseWrite();

            yield break;
        }
    }
}
