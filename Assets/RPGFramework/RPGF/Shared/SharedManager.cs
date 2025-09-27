using RPGF.Core.Architecture;

namespace RPGF.Shared
{
    public class SharedManager : KernelManagerBase
    {
        public static SharedManager Instance;

        public MessageBoxManager MessageBox;
        public ChoiceBoxManager ChoiceBox;
        public MediaManager Media;

        public override void Initialize()
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