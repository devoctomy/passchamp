using Newtonsoft.Json.Linq;

namespace devoctomy.Passchamp.Core.Graph.Services;

public interface INodesJsonParserService
{
    NodesJsonParserServiceParseResults Parse(JArray json);
}
