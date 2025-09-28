using System.Collections;

namespace RPGF.EventSystem.Default
{
    public class StartAction : ActionBase
    {
        public StartAction() : base("Start")
        {
        }

        public override IEnumerator ActionCoroutine()
        {
            yield break;
        }

        public override string GetInfo()
        {
            return "Старт события";
        }

        public override string GetHeader()
        {
            return "Старт";
        }
    }
}
