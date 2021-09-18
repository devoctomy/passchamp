using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Graph.Services
{
    public interface IGraphPinOutputFunction
    {
        bool IsApplicable(string key);
        IPin Execute(
            string curNodeKey,
            string value,
            IReadOnlyDictionary<string, INode> nodes);
    }
}
