using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Graph.Services
{
    public interface INodesJsonParserService
    {
        Dictionary<string, INode> Parse(
            JArray json,
            out string startNodeKey);
    }
}
