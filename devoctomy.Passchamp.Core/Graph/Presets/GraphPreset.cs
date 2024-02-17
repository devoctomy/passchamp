using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Graph.Presets;

public class GraphPreset
{
    public string Author { get; set; }
    public string Description { get; set; }
#if DEBUG
    public bool Debug { get; set; }
#endif

    public string StartKey { get; set; }

    public List<NodeRef> OrderedNodes { get; set; }
    public List<NodeRef> UnorderedNodes { get; set; }

    public Dictionary<string, IPin> InputPins { get; set; }
    public Dictionary<string, IPin> OutputPins { get; set; }

    public List<NodeConnection> Connections { get; set; }
}
