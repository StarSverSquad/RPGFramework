using RPGF.Battle;
using RPGF.Domain.DI;
using RPGF.EventSystem;
using RPGF.EventSystem.Attributes;
using System;
using System.Collections;
using UnityEngine;

namespace RPGF.Actions
{
    [GenerateActionNode("Сменить музыку битвы", contextMenuPath: "Битва/Сменить музыку битвы")]
    [Serializable]
    public class ChangeBattleBGMAction : ActionBase
    {
        [Inject]
        private readonly BattleAudioManager _audio;

        [ActionFieldOption("Трек:")]
        public AudioClip Clip;
        [ActionFieldOption("Громкость:")]
        public float Volume;

        public ChangeBattleBGMAction() : base()
        {
            Clip = null;
            Volume = 1.0f;
        }

        public override IEnumerator ActionCoroutine()
        {
            if (BattleManager.IsBattle)
                _audio.PlayMusic(Clip, Volume);

            yield break;
        }
    }
}