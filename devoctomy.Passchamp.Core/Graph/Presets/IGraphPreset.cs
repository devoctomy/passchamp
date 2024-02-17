using devoctomy.Passchamp.Core.Enums;
using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Graph.Presets;

public interface IGraphPreset
{
    public GraphContext Context { get; }
    public bool Default { get; }
    public string Author { get; }
    public string Description { get; }
    public string StartKey { get; }
    public List<NodeRef> OrderedNodes { get; }
    public List<NodeRef> UnorderedNodes { get; }
    public Dictionary<string, IPin> InputPins { get; }
    public Dictionary<string, IPin> OutputPins { get; }
    public List<NodeConnection> Connections { get; }
}
