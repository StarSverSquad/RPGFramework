using RPGF.Core;
using UnityEngine;

namespace RPGF.Shared
{
    public class SharedManager : KernelManagerBase
    {
        public static SharedManager Instance;

        public ChoiceDialogManager ChoiceDialog;
        public MessageBoxManager MessageDialog;
        public MediaManager Media;

        public Canvas Canvas;

        private LocalManager Local => LocalManager.Instance;

        public override void Initialize()
        {
            Instance = this;

            Local.DI.AddSignleton(this);

            InitializeChild();
        }

        public override void InitializeChild()
        {
            MessageDialog.Initialize();
            Local.DI.AddSignleton(MessageDialog);

            ChoiceDialog.Initialize();
            Local.DI.AddSignleton(ChoiceDialog);

            Media.Initialize();
            Local.DI.AddSignleton(Media);
        }
    }
}