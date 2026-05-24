using System;
using UnityEngine;

namespace RPGF.GUI.Interfaces
{
    public interface IGUIElement : IGUIWidget, IDisposable
    {
        public bool Focused { get; }

        public void SetFocus(bool focus);
    }
}
