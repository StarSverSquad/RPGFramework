using RPGF.EventSystem;
using System;
using UnityEngine;

namespace RPGF.EventSystem
{
    [Serializable]
    public class NextAction
    {
        [SerializeReference]
        public ActionBase Action;

        public string Tag;

        public bool IsNext;
    }
}
