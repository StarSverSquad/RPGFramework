namespace RPGF.GUI
{
    public interface IGUIBlock
    {
        public void Preview();
        public void Next(IGUIBlock gUIBlock);

        public void Activate();
        public void Diativate();
    }
}
