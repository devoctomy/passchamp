using System;
using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Graph.Presets;

public class NodeRef
{
    public string Key { get; set; }
    public Type NodeType { get; set; }
    public string NextKey { get; set; }
    public Dictionary<string, IPin> InputPins { get; set; }
}
