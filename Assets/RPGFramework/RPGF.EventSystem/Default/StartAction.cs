using System;
using System.Collections;

namespace RPGF.EventSystem.Default
{
    [Serializable]
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
