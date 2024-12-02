public class ActionNodeWrapper<T> : ActionNode 
    where T : GraphActionBase
{
    public T Action => action as T;

    public ActionNodeWrapper(T action) : base(action) { }

    public override void UIContructor()
    {
        
    }

    public virtual string GetContextualMenuPath()
    {
        return $"Разное/{GetType().Name}";
    }
}
