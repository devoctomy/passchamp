using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph
{
    public interface IGraph
    {
        Dictionary<string, NodeBase> Nodes { get; }
        string StartKey { get; }
        T GetNode<T>(string key) where T : NodeBase;
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
