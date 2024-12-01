using System.Collections;
using UnityEngine;

public class ChangeBattleBGMAction : GraphActionBase
{
    public AudioClip Clip;
    public float Volume;

    public ChangeBattleBGMAction() : base("ChangeBattleBGM")
    {
        Clip = null;
        Volume = 1.0f;
    }

    public override IEnumerator ActionCoroutine()
    {
        if (BattleManager.IsBattle)
            BattleManager.Instance.BattleAudio.PlayMusic(Clip, Volume);

        yield break;
    }

    public override string GetHeader()
    {
        return "Сменить музыку битвы";
    }
}