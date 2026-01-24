using System;

namespace RPGF.EventSystem.Graph
{
    [Serializable]
    public class GraphEvent : Event
    {
        public GraphEventMeta Meta = new();

        public event Action GraphChanged;

        public void DispathGraphChanges() => GraphChanged?.Invoke();
    }
}