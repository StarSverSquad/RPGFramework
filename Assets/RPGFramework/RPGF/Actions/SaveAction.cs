using RPGF;
using RPGF.EventSystem;
using System.Collections;

public class SaveAction : GraphActionBase
{
    public int slotId;

    public SaveAction() : base("GameSave")
    {
        slotId = 0;
    }

    public override IEnumerator ActionCoroutine()
    {
        GlobalManager.Instance.SaveLoad.GameSave(slotId);

        yield break;
    }

    public override string GetHeader()
    {
        return "Сохранить игру";
    }
}
