using System;

namespace RPGF.GUI.Interfaces
{
    public interface IGUIBlock : IDisposable
    {
        public bool IsFocused { get; }
        public bool IsActivated { get; }

        public IGUIManager Manager { get; }

        public void Initialize(IGUIManager manager);

        public void Preview();
        public void Next(IGUIBlock gUIBlock);

        public void Activate();
        public void Deactivate();

        public void SetFocus(bool focus);
    }
}
