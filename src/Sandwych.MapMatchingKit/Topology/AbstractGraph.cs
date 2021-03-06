﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Sandwych.MapMatchingKit.Topology
{

    public abstract class AbstractGraph<TEdge> : QuickGraph.AdjacencyGraph<long, TEdge>, IGraph<TEdge>
        where TEdge : AbstractGraphEdge<TEdge>
    {
        private bool _constructed = false;
        private readonly Dictionary<long, TEdge> _edgeMap = new Dictionary<long, TEdge>();

        public AbstractGraph(IEnumerable<TEdge> edges)
        {
            if (edges == null)
            {
                throw new ArgumentNullException(nameof(edges));
            }

            this.AddVerticesAndEdgeRange(edges);
            foreach (var e in edges)
            {
                _edgeMap.Add(e.Id, e);
            }

            this.Construct();
        }

        public TEdge GetEdge(long id) => _edgeMap[id];

        public IReadOnlyDictionary<long, TEdge> EdgeMap => _edgeMap;

        protected virtual void Construct()
        {
            if (_constructed)
            {
                throw new InvalidOperationException();
            }

            var map = new Dictionary<long, IList<TEdge>>();

            foreach (var edge in this.EdgeMap.Values)
            {
                if (!map.ContainsKey(edge.Source))
                {
                    map[edge.Source] = new List<TEdge>() { edge };
                }
                else
                {
                    map[edge.Source].Add(edge);
                }
            }

            IList<TEdge> successors = null;
            foreach (var edges in map.Values)
            {
                for (int i = 1; i < edges.Count; ++i)
                {
                    var prevEdge = edges[i - 1];
                    prevEdge.Neighbor = edges[i];

                    prevEdge.Successor = map.TryGetValue(prevEdge.Target, out successors) ? successors.First() : default;
                }

                var lastEdge = edges.Last();
                lastEdge.Neighbor = edges.First();
                lastEdge.Successor = map.TryGetValue(lastEdge.Target, out successors) ? successors.First() : default;
            }

            _constructed = true;
        }
    }
}
