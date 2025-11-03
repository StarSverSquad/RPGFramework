using RPGF.EventSystem.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGF.EventSystem.Default
{
    public class EndAction : ActionBase
    {
        public EndAction() : base()
        {
        }

        public override IEnumerator ActionCoroutine()
        {
            yield break;
        }
    }
}
