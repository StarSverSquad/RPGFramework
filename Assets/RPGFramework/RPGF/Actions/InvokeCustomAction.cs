using RPGF.EventSystem;
using RPGF.EventSystem.Attributes;
using System;
using System.Collections;

namespace RPGF.Actions
{
    [Serializable]
    [GenerateActionNode("Запуск самописного события", contextMenuPath: "Прочее/Запуск самописного события")]
    public class InvokeCustomAction : ActionBase
    {
        [ActionFieldOption("Событие", AllowSceneObjects = true)]
        public CustomActionBase act;

        public InvokeCustomAction() : base()
        {
        }

        public override IEnumerator ActionCoroutine()
        {
            yield return act.ActionCoroutine();
        }
    }
}