using System.Collections.Generic;

namespace RPGF.GUI
{
    public interface IGUIManager
    {
        public void NextBlock(IGUIBlock block);
        public void PreviewBlock();

        public void Open();
        public void Close();
    }
}
