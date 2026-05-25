using RPGF.Core.SaveLoad;
using RPGF.Domain.DI;
using RPGF.EventSystem;
using RPGF.EventSystem.Attributes;
using System;
using System.Collections;

namespace RPGF.Actions
{
    [GenerateActionNode("Сохранить игру", contextMenuPath: "Система/Сохранить игру")]
    [Serializable]
    public class SaveAction : ActionBase
    {
        [Inject]
        private readonly SaveLoadService _saveLoad = null!;

        [ActionFieldOption("ID слота:")]
        public int slotId;

        public SaveAction() : base()
        {
            slotId = 0;
        }

        public override IEnumerator ActionCoroutine()
        {
            _saveLoad.GameSave(slotId);

            yield break;
        }
    }
}
