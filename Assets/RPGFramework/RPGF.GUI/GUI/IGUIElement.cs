using System;
using UnityEngine;

namespace RPGF.GUI
{
    public interface IGUIElement : IDisposable
    {
        public RectTransform RectTransform { get; }
    }
}
