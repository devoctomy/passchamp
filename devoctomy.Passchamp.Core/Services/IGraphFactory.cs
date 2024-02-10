using devoctomy.Passchamp.Core.Enums;
using devoctomy.Passchamp.Core.Graph;
using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Services;

public interface IGraphFactory
{
    public IGraph LoadNative(
        GraphContext context,
        NativeGraphs graph,
        params KeyValuePair<string, object>[] parameters);
}
