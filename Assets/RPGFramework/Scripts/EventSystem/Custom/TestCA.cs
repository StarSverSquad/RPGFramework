using System;
using System.Collections;
using UnityEngine;

public class TestCA : CustomActionBase
{
    protected override IEnumerator ActionCoroutine()
    {
        ExplorerManager.PlayerMovement.TranslateBySpeed(new Vector2(-2, 0), 3);

        yield return new WaitWhile(() => ExplorerManager.instance.playerManager.movement.IsAutoMoving);
    }
}