using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Graph.Services
{
    public interface IPinsJsonParserService
    {
        Dictionary<string, IDataPin> Parse(JArray json);
    }
}
