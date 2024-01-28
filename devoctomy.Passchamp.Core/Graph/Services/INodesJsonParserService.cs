using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Graph.Services;

public interface INodesJsonParserService
{
    NodesJsonParserServiceParseResults Parse(JArray json);
}
