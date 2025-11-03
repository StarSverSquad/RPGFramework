using RPGF.EventSystem.Attributes;
using System.Collections;

namespace RPGF.EventSystem.Default
{
    public class StartAction : ActionBase
    {
        public StartAction() : base()
        {
        }

        public override IEnumerator ActionCoroutine()
        {
            yield break;
        }
    }
}
