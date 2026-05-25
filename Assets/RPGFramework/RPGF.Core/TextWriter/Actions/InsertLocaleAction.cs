using RPGF.Core.Localization;
using RPGF.Core.TextWriter.Abstractions;
using RPGF.Domain.DI;
using RPGF.Domain.TP;
using RPGF.Domain.TP.Abstractions;
using System.Collections;

namespace RPGF.Core.TextWriter.Actions
{
    [UseTextAction(@"^%(\w|_)+$", TextActionType.Single)]
    public class InsertLocaleAction : TextWriterActionBase
    {
        [Inject]
        private readonly LocalizationService _localization = null!;

        public override IEnumerator Action(TextActionParams @params)
        {
            string tag = @params.Tag[1..];

            ReturnText = _localization.GetLocale(tag);

            yield break;
        }
    }
}
