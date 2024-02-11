using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Presets;

public class NodeRef
{
    public string Key { get; set; }
    public Type NodeType { get; set; }
    public string NextKey { get; set; }
    public Dictionary<string, IPin> InputPins { get; set; }
}
