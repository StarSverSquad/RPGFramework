namespace RPGF.Shared
{
    public class SharedManager : ContentManagerBase, IManagerInitialize
    {
        public static SharedManager Instance;

        public MessageBoxManager MessageBox;
        public ChoiceBoxManager ChoiceBox;
        public MediaManager Media;

        public void Initialize()
        {
            Instance = this;

            InitializeChild();
        }

        public override void InitializeChild()
        {
            MessageBox.Initialize();
            Media.Initialize();
        }
    }
}