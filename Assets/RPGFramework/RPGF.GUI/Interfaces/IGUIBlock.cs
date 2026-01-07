using System;

namespace RPGF.GUI.Interfaces
{
    public interface IGUIBlock : IDisposable
    {
        public void Initialize(IGUIManager manager);

        public void Preview();
        public void Next(IGUIBlock gUIBlock);

        public void Activate();
        public void Diativate();

        public void SetFocus(bool focus);
    }
}
