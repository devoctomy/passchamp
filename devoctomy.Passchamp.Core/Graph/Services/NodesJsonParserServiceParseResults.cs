using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Graph.Services
{
    public class NodesJsonParserServiceParseResults
    {
        public string StartNodeKey { get; set; }
        public Dictionary<string, INode> Nodes { get; set; }
    }
}
