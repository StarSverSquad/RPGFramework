using System;
using System.Collections;
using UnityEngine;

public class MessageAction : GraphActionBase
{
    public MessageInfo message;

    public MessageAction() : base("Message")
    {
        message = new MessageInfo();
    }

    public override IEnumerator ActionCoroutine()
    {
        CommonManager.instance.messageBox.Write(message);

        yield return new WaitWhile(() => CommonManager.instance.messageBox.IsWriting);
    }

    public override string GetHeader()
    {
        return "Сообщение";
    }
}