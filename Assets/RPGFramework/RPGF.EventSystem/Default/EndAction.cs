using System;
using System.Collections;

namespace RPGF.EventSystem.Default
{
    [Serializable]
    public class EndAction : ActionBase
    {
        public EndAction() : base()
        {
            Nexts.Clear();
        }

        public override IEnumerator ActionCoroutine()
        {
            yield break;
        }
    }
}
