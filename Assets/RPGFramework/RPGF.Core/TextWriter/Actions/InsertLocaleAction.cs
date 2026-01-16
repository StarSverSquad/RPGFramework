using RPGF.Core.Localization;
using RPGF.Domain.DI;
using System.Collections;

namespace RPGF.Core.TextWriter.Actions
{
    [UseTextWriterAction(@"^%(\w|_)+$", TextActionType.Instance)]
    public class InsertLocaleAction : TextActionBase
    {
        [Inject]
        private readonly LocalizationService _localization;

        protected override IEnumerator Action(TextActionParams @params)
        {
            string tag = @params.Tag[1..];

            ReturnText = _localization.GetLocale(tag);

            yield break;
        }
    }
}
