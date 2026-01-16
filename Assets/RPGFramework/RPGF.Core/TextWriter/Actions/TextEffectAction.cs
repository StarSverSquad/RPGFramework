using System.Text.RegularExpressions;

namespace RPGF.Core.TextWriter.Actions
{
    public class TextEffectAction : TextActionBase
    {
        public TextEffectAction(Regex regex, ActionType actType) : base(regex, actType)
        {
        }
    }
}
