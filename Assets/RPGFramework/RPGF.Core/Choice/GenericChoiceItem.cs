namespace RPGF.Core.Choice
{
    public class GenericChoiceItem<T> : ChoiceItem
    {
        public T Value { get; set; }

        public GenericChoiceItem(T value)
        {
            Value = value;
        }
    }
}
