using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Graph.Services
{
    public interface IPinsJsonParserService
    {
        Dictionary<string, IPin> Parse(JArray json);
    }
}
