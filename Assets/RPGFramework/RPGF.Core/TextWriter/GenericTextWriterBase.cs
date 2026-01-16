namespace RPGF.Core.TextWriter
{
    public abstract class GenericTextWriterBase<T> : TextWriterBase
        where T : WriterMessage
    {
        public T Message => BaseMessage as T;

        public virtual void Write(T message)
        {
            base.InvokeWrite(message);
        }
    }
}
