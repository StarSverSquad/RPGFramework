using RPGF.Domain.DI;
using RPGF.EventSystem;
using RPGF.Shared;
using System.Collections;
using UnityEngine;

namespace RPGF.Actions
{
    public class MessageAction : ActionBase
    {
        [Inject]
        private readonly MessageBoxManager _messageBox;

        public MessageInfo message;

        public MessageAction() : base()
        {
            message = new MessageInfo();
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
}