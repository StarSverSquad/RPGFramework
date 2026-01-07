namespace RPGF.GUI.Interfaces
{
    public interface IGUIInteractable : IGUIElement
    {
        public void SetFocus(bool focus);

        public void Select();
        public void Cancel();
    }
}
