using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GraphEventMeta
{
    [Serializable]
    public class NodeMeta
    {
        public string guid;

        public int actionIndex;

        public Vector2 position;
    }
    [Serializable]
    public class EdgeMeta
    {
        public string inputPortName;
        public string inputNodeGUID;

        public string outputPortName;
        public string outputNodeGUID;
    }

    public List<EdgeMeta> edges;
    public List<NodeMeta> nodes;

    public GraphEventMeta()
    {
        edges = new List<EdgeMeta>();
        nodes = new List<NodeMeta>();
    }
}
