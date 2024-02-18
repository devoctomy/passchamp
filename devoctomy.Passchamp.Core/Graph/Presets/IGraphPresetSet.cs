using System;

namespace devoctomy.Passchamp.Core.Graph.Presets;

public interface IGraphPresetSet
{
    public Version Version { get; }
    public bool Default { get; }
    public string Id { get; }
    public string Author { get; }
    public string Description { get; }
    public IGraphPreset EncryptPreset { get; }
    public IGraphPreset DecryptPreset { get; }
}
