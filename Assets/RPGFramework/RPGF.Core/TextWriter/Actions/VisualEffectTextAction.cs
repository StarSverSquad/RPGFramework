using RPGF.Core.TextWriter.Abstrations;
using RPGF.Domain.TP;
using RPGF.Domain.TP.Abstractions;
using System.Collections;

namespace RPGF.Core.TextWriter.Actions
{
    [UseTextAction("^effect$", TextActionType.Scoped)]
    public class VisualEffectTextAction : TextWriterActionBase
    {
        /// <summary>
        /// pending params: 
        ///     type: [name]
        ///     
        ///     names: ["tence", "wave"]
        /// </summary>
        /// <returns></returns>
        public override IEnumerator Action(TextActionParams @params)
        {
            @params.Tag += "t";

            yield break;
        }
    }
}
