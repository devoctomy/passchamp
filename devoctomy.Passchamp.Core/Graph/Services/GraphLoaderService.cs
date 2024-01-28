using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Services;

public class GraphLoaderService : IGraphLoaderService
{
    private readonly IInputPinsJsonParserService _inputPinsJsonParserService;
    private readonly IOutputPinsJsonParserService _outputPinsJsonParserService;
    private readonly INodesJsonParserService _nodeJsonParserService;
    private readonly IEnumerable<IGraphPinPrepFunction> _pinPrepFunctions;

    public GraphLoaderService(
        IInputPinsJsonParserService inputPinsJsonParserService,
        IOutputPinsJsonParserService outputPinsJsonParserService,
        INodesJsonParserService nodeJsonParserService,
        IEnumerable<IGraphPinPrepFunction> pinPrepFunctions)
    {
        _inputPinsJsonParserService = inputPinsJsonParserService;
        _outputPinsJsonParserService = outputPinsJsonParserService;
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

        var settings = json["Settings"] != null ? JsonConvert.DeserializeObject<GraphSettings>(json["Settings"].ToString()) : new GraphSettings();
        var inputPins = _inputPinsJsonParserService.Parse(json["InputPins"].Value<JArray>());
        var outputPins = _outputPinsJsonParserService.Parse(json["OutputPins"].Value<JArray>());
        var parseResults = _nodeJsonParserService.Parse(json["Nodes"].Value<JArray>());

        return new Graph(
            settings,
            inputPins,
            outputPins,
            parseResults.Nodes,
            parseResults.StartNodeKey,
            outputMessage,
            _pinPrepFunctions);
    }
}
