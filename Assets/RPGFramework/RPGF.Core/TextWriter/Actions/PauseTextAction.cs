using RPGF.Core.TextWriter.Abstractions;
using RPGF.Domain.TP;
using RPGF.Domain.TP.Abstractions;
using System.Collections;

namespace RPGF.Core.TextWriter.Actions
{
    [UseTextAction(@"^!$", TextActionType.Single)]
    public class PauseTextAction : TextWriterActionBase
    {
        public override IEnumerator Action(TextActionParams @params)
        {
            TextWriter.PauseWrite();

            yield break;
        }
    }
}
