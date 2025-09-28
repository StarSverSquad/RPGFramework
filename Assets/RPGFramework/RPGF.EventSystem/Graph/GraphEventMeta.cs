using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGF.EventSystem.Graph
{
    [Serializable]
    public class GraphEventMeta
    {
        [Serializable]
        public class NodeMeta
        {
            public string guid;

            [SerializeReference]
            public ActionBase action;

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
            edges = new();
            nodes = new();
        }
    }
}