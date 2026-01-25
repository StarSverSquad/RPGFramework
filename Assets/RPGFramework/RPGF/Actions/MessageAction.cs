using RPGF.Domain.DI;
using RPGF.EventSystem;
using RPGF.Shared;
using System;
using System.Collections;
using UnityEngine;

namespace RPGF.Actions
{
    [Serializable]
    public class MessageAction : ActionBase
    {
        [Inject]
        private readonly MessageBoxManager _messageBox;

        public MessageBoxInfo message;

        public MessageAction() : base()
        {
            message = new MessageBoxInfo();
        }

        public override IEnumerator ActionCoroutine()
        {
            _messageBox.Write(message);

            yield return new WaitWhile(() => _messageBox.IsWriting);
        }

        public override ActionBase Clone()
        {
            return new MessageAction()
            {
                message = new MessageBoxInfo()
                {
                    text = message.text,
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
}