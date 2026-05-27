namespace RPGF.GUI.Interfaces
{
    public interface IGUIManager
    {
        public void NextBlock(IGUIBlock block);
        public void PreviousBlock();

        public void Open();
        public void Open(IGUIBlock block);
        public void Close();
    }
}
