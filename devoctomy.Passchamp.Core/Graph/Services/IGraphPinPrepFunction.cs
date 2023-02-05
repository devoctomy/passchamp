using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Graph.Services;

public interface IGraphPinPrepFunction
{
    bool IsApplicable(string key);
    IPin Execute(
        string curNodeKey,
        string value,
        IReadOnlyDictionary<string, IPin> inputPins,
        IReadOnlyDictionary<string, INode> nodes);
}
