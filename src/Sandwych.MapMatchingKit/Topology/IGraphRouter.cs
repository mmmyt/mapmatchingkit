﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Sandwych.MapMatchingKit.Topology
{
    public interface IGraphRouter<TEdge, P>
        where TEdge : IGraphEdge<TEdge>
        where P : IEdgePoint<TEdge>, IEquatable<P>
    {

        IEnumerable<TEdge> Route(P source, P target,
            Func<TEdge, double> cost, Func<TEdge, double> bound = null, double max = double.NaN);

        IReadOnlyDictionary<P, IEnumerable<TEdge>> Route(P source, IEnumerable<P> targets,
            Func<TEdge, double> cost, Func<TEdge, double> bound = null, double max = double.NaN);

        IReadOnlyDictionary<P, (P, IEnumerable<TEdge>)> Route(IEnumerable<P> sources, IEnumerable<P> targets,
            Func<TEdge, double> cost, Func<TEdge, double> bound = null, double max = double.NaN);
    }


}
