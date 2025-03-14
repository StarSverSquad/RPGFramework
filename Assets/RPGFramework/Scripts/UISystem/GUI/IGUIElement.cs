namespace RPGF.GUI
{
    public interface IGUIElement
    {
        public void SetFocus(bool focus);

        public void Select();
        public void Cancel();
    }
}
