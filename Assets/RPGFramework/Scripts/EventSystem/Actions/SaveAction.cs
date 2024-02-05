using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAction : GraphActionBase
{
    public int slotId;

    public SaveAction() : base("Save")
    {
        slotId = 0;
    }

    public override IEnumerator ActionCoroutine()
    {
        GameManager.Instance.SaveLoad.Save(slotId);

        yield break;
    }

    public override string GetHeader()
    {
        return "Сохранить игру";
    }
}
