using RPGF.Core;

namespace RPGF.Shared
{
    public class SharedManager : KernelManagerBase
    {
        public static SharedManager Instance;

        public ChoiceBoxManager ChoiceBox;
        public MessageBoxManager MessageBox;
        public MediaManager Media;

        private LocalManager Local => LocalManager.Instance;

        public override void Initialize()
        {
            Instance = this;

            Local.DI.AddSignleton(ChoiceBox);

            InitializeChild();
        }

        public override void InitializeChild()
        {
            MessageBox.Initialize();
            Local.DI.AddSignleton(MessageBox);

            Media.Initialize();
            Local.DI.AddSignleton(Media);
        }
    }
}