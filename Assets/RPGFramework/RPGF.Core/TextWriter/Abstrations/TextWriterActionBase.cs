using RPGF.Domain.TP.Abstractions;
using UnityEngine;

namespace RPGF.Core.TextWriter.Abstrations
{
    public abstract class TextWriterActionBase : TextActionBase
    {
        public TextWriterBase TextWriter { get; set; }

        public Coroutine Invoke(MonoBehaviour listner, TextActionParams @params)
        {
            return listner.StartCoroutine(Action(@params));
        }
    }
}
