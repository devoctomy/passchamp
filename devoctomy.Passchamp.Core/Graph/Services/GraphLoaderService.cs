using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Services
{
    public class GraphLoaderService : IGraphLoaderService
    {
        private readonly IPinsJsonParserService _pinsJsonParserService;
        private readonly INodesJsonParserService _nodeJsonParserService;
        private readonly IEnumerable<IGraphPinPrepFunction> _pinPrepFunctions;
        private readonly IEnumerable<IGraphPinOutputFunction> _pinOutputFunctions;

        public GraphLoaderService(
            IPinsJsonParserService pinsJsonParserService,
            INodesJsonParserService nodeJsonParserService,
            IEnumerable<IGraphPinPrepFunction> pinPrepFunctions,
            IEnumerable<IGraphPinOutputFunction> pinOutputFunctions)
        {
            _pinsJsonParserService = pinsJsonParserService;
            _nodeJsonParserService = nodeJsonParserService;
            _pinPrepFunctions = pinPrepFunctions;
            _pinOutputFunctions = pinOutputFunctions;
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

            var settings = json["Settings"] != null ? JsonConvert.DeserializeObject<GraphSettings>(json["Settings"].ToString()) : new GraphSettings();
            var inputPins = _pinsJsonParserService.Parse(json["InputPins"].Value<JArray>());
            var outputPins = _pinsJsonParserService.Parse(json["OutputPins"].Value<JArray>());
            var nodes = _nodeJsonParserService.Parse(
                json["Nodes"].Value<JArray>(),
                out var startNodeKey);

            var pop = nodes.Values.ToList();

            return new Graph(
                settings,
                inputPins,
                outputPins,
                nodes,
                startNodeKey,
                outputMessage,
                _pinPrepFunctions,
                _pinOutputFunctions);
        }
    }
}
