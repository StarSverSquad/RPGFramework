using System.Collections;
using System.Text.RegularExpressions;

namespace RPGF.Core.TextWriter.Actions
{
    public class PauseTextAction : TextActionBase
    {
        public PauseTextAction() : base(new Regex(@"^!$"), ActionType.Instance) { }

        protected override IEnumerator Action()
        {
            TextWriter.PauseWrite();

            yield break;
        }
    }
}
