namespace RPGF.GUI.Interfaces
{
    public interface IGUIManager
    {
        public void NextBlock(IGUIBlock block);
        public void PreviewBlock();

        public void Open();
        public void Close();
    }
}
