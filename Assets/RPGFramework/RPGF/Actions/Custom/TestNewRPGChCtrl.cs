using RPGF.Core.Character;
using RPGF.EventSystem;
using System.Collections;
using UnityEngine;

namespace RPGF.Actions
{
    class TestNewRPGChCtrl : CustomActionBase
    {
        public PlayableCharacterModelController rPGCharacter;

        public override IEnumerator ActionCoroutine()
        {
            rPGCharacter.MoveToRelative(new Vector2(3, 0), 3f);

            yield return new WaitForSeconds(1f);

            rPGCharacter.PauseMove();

            yield return new WaitForSeconds(1f);

            rPGCharacter.ResumeMove();

            yield return new WaitWhile(() => rPGCharacter.IsMove);
        }
    }
}