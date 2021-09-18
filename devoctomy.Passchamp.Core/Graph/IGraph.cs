﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph
{
    public interface IGraph
    {
        public delegate void GraphOutputMessageDelegate(INode node, string message);

        GraphOutputMessageDelegate OutputMessage { get; set; }
        IReadOnlyDictionary<string, IPin> InputPins { get; }
        IReadOnlyList<string> ExecutionOrder { get; }
        IReadOnlyDictionary<string, INode> Nodes { get; }
        IReadOnlyDictionary<INode, string> NodeKeys { get; }
        Dictionary<string, object> ExtendedParams { get; }
        string StartKey { get; }
        T GetNode<T>(string key) where T : INode;
        Task ExecuteAsync(CancellationToken cancellationToken);
        public void BeforeExecute(INode node);
    }
}
