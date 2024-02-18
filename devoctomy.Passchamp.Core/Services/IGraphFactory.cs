using devoctomy.Passchamp.Core.Enums;
using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Presets;
using System;
using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Services;

public interface IGraphFactory
{
    public (IGraph encrypt, IGraph decrypt) LoadPresetSet(
        IGraphPresetSet presetSet,
        Func<Type, INode> instantiateNode,
        Dictionary<string, object> parameters);

    public IGraph LoadPreset(
        IGraphPreset preset,
        Func<Type, INode> instantiateNode,
        Dictionary<string, object> parameters);

    public IGraph LoadNative(
        GraphContext context,
        NativeGraphs graph,
        Func<Type, INode> instantiateNode,
        Dictionary<string, object> parameters);
}
