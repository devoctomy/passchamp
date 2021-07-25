using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph
{
    public interface INode
    {
        Dictionary<string, PropertyInfo> InputPinsProperties { get; }
        Dictionary<string, PropertyInfo> OutputPinsProperties { get; }
        Dictionary<string, IPin> Input { get; }
        Dictionary<string, IPin> Output { get; }
        public string NextKey { get; set; }
        public bool Executed { get; }
        Task ExecuteAsync(
            IGraph graph,
            CancellationToken cancellationToken);
        void PrepareInputDataPin(
            string key,
            Type valueType,
            bool validate = true);
        void PrepareOutputDataPin(
            string key,
            Type valueType,
            bool validate = true);
        Task ExecuteNextAsync(
            IGraph graph,
            CancellationToken cancellationToken);
        IDataPin<T> GetInput<T>(string key);
        IPin GetInput(string key, Type type);
        IDataPin<T> GetOutput<T>(string key);
        IPin GetOutput(string key, Type type);
    }
}
