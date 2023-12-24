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
        CommonManager.Instance.MessageBox.Write(message);

        yield return new WaitWhile(() => CommonManager.Instance.MessageBox.IsWriting);
    }

    public override string GetHeader()
    {
        return "Сообщение";
    }

    public override object Clone()
    {
        return new MessageAction()
        {
            message = new MessageInfo()
            {
                text = message.text,
                clear = message.clear,
                closeWindow = message.closeWindow,
                image = message.image,
                speed = message.speed,
                letterSound = message.letterSound,
                name = message.name,
                position = message.position,
                wait = message.wait
            }
        };
    }
}