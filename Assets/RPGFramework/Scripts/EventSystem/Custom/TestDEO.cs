using System.Collections;
using UnityEngine;

public class TestDEO : CustomActionBase
{
    public Sprite olek;
    public RPGCharacterControllerLegacy deo;

    protected override IEnumerator ActionCoroutine()
    {
        CommonManager.Instance.MessageBox.Write(new MessageInfo
        {
            wait = true,
            clear = true,
            image = olek,
            name = "Олежа",
            position = MessageBoxManager.DialogBoxPosition.Bottom,
            text = "Лан пока",
            closeWindow = true,
        });

        yield return new WaitWhile(() => CommonManager.Instance.MessageBox.IsWriting);

        deo.TranslateByTime(new Vector2(1.5f, 0), 2);

        yield return new WaitWhile(() => deo.IsMove);

        yield return new WaitForSeconds(1);

        deo.RotateTo(CommonDirection.Down);

        yield return new WaitForSeconds(1);

        CommonManager.Instance.MessageBox.Write(new MessageInfo
        {
            wait = true,
            clear = true,
            image = olek,
            name = "Олежа",
            position = MessageBoxManager.DialogBoxPosition.Bottom,
            closeWindow = true,
            text = "Лан..."
        });

        yield return new WaitWhile(() => CommonManager.Instance.MessageBox.IsWriting);

        deo.TranslateByTime(new Vector2(-1.5f, 0), 2);

        yield return new WaitForSeconds(1);

        deo.PauseMove();

        CommonManager.Instance.MessageBox.Write(new MessageInfo
        {
            wait = true,
            clear = true,
            image = olek,
            name = "Олежа",
            position = MessageBoxManager.DialogBoxPosition.Bottom,
            closeWindow = true,
            text = "Хотя<!>\nНе..."
        });

        yield return new WaitWhile(() => CommonManager.Instance.MessageBox.IsWriting);

        deo.UnpauseMove();

        yield return new WaitWhile(() => deo.IsMove);

        deo.SetDefault();
    }
}