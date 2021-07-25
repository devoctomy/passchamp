﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph
{
    public interface IGraph
    {
        Dictionary<string, IPin> Pins { get; }
        IReadOnlyList<string> ExecutionOrder { get; }
        Dictionary<string, INode> Nodes { get; }
        string StartKey { get; }
        T GetNode<T>(string key) where T : INode;
        Task ExecuteAsync(CancellationToken cancellationToken);
        public void BeforeExecute(INode node);
    }
}
