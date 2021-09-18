using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Services
{
    public class GraphLoaderService : IGraphLoaderService
    {
        private readonly IPinsJsonParserService _pinsJsonParserService;
        private readonly INodesJsonParserService _nodeJsonParserService;
        private readonly IEnumerable<IGraphPinPrepFunction> _pinPrepFunctions;

        public GraphLoaderService(
            IPinsJsonParserService pinsJsonParserService,
            INodesJsonParserService nodeJsonParserService,
            IEnumerable<IGraphPinPrepFunction> pinPrepFunctions)
        {
            _pinsJsonParserService = pinsJsonParserService;
            _nodeJsonParserService = nodeJsonParserService;
            _pinPrepFunctions = pinPrepFunctions;
        }

        public Task<IGraph> LoadAsync(
            string graphJsonFile,
            IGraph.GraphOutputMessageDelegate outputMessage,
            CancellationToken cancellationToken)
        {
            return LoadAsync(
                File.OpenRead(graphJsonFile),
                outputMessage,
                cancellationToken);
        }

        public async Task<IGraph> LoadAsync(
            Stream graphJson,
            IGraph.GraphOutputMessageDelegate outputMessage,
            CancellationToken cancellationToken)
        {
            using var streamReader = new StreamReader(
                graphJson,
                leaveOpen: true);
            using var jsonReader = new JsonTextReader(streamReader);

            var json = await JObject.ReadFromAsync(
                    jsonReader,
                    cancellationToken);

            var inputPins = _pinsJsonParserService.Parse(json["InputPins"].Value<JArray>());
            var outputPins = _pinsJsonParserService.Parse(json["OutputPins"].Value<JArray>());
            var nodes = _nodeJsonParserService.Parse(
                json["Nodes"].Value<JArray>(),
                out var startNodeKey);

            return new Graph(
                inputPins,
                outputPins,
                nodes,
                startNodeKey,
                outputMessage,
                _pinPrepFunctions);
        }
    }
}
