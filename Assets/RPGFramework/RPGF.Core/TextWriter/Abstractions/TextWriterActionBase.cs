using RPGF.Domain.TP.Abstractions;
using UnityEngine;

namespace RPGF.Core.TextWriter.Abstractions
{
    public abstract class TextWriterActionBase : TextActionBase
    {
        public TextWriterBase TextWriter { get; set; }

        public Coroutine Invoke(MonoBehaviour listener, TextActionParams @params)
        {
            return listener.StartCoroutine(Action(@params));
        }
    }
}
