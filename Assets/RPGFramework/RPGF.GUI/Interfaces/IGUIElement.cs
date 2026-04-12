using System;
using UnityEngine;

namespace RPGF.GUI.Interfaces
{
    public interface IGUIElement : IDisposable
    {
        public RectTransform RectTransform { get; }

        public bool Focused { get; }

        public void SetFocus(bool focus);
    }
}
