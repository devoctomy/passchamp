using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Services
{
    public class GraphLoaderService : IGraphLoaderService
    {
        private readonly IPinsJsonParserService _pinsJsonParserService;
        private readonly INodesJsonParserService _nodeJsonParserService;

        public GraphLoaderService(
            IPinsJsonParserService pinsJsonParserService,
            INodesJsonParserService nodeJsonParserService)
        {
            _pinsJsonParserService = pinsJsonParserService;
            _nodeJsonParserService = nodeJsonParserService;
        }

        public async Task<IGraph> LoadAsync(
            Stream graphJson,
            CancellationToken cancellationToken)
        {
            using var streamReader = new StreamReader(
                graphJson,
                leaveOpen: true);
            using var jsonReader = new JsonTextReader(streamReader);

            var json = await JObject.ReadFromAsync(
                    jsonReader,
                    cancellationToken);

            var pins = _pinsJsonParserService.Parse(json["Pins"].Value<JArray>());
            var nodes = _nodeJsonParserService.Parse(
                json["Nodes"].Value<JArray>(),
                out var startNodeKey);

            return new Graph(
                pins,
                nodes,
                startNodeKey);
        }
    }
}
