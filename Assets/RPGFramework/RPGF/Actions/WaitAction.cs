using RPGF.EventSystem;
using RPGF.EventSystem.Attributes;
using System;
using System.Collections;
using UnityEngine;

namespace RPGF.Actions
{
    [GenerateActionNode("Ждать", contextMenuPath: "Система/Ждать")]
    [Serializable]
    public class WaitAction : ActionBase
    {
        [ActionFieldOption("Врема (сек.):")]
        public float time;

        public WaitAction() : base()
        {
            time = 1;
        }

        public override IEnumerator ActionCoroutine()
        {
            yield return new WaitForSeconds(time);
        }

        public override ActionBase Clone()
        {
            return new WaitAction()
            {
                time = time
            };
        }
    }
}