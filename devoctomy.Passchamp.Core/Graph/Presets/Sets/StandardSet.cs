using System;
using System.Collections.Generic;
using System.Linq;

namespace devoctomy.Passchamp.Core.Graph.Presets.Sets;

public class StandardSet(IEnumerable<IGraphPreset> graphPresets) : IGraphPresetSet
{
    public Version Version => new(1, 0);
    public bool Default => true;
    public string Id => "E17E1F18-16CC-4DD8-8FB8-1AAF0153168D";
    public string Author => "devoctomy";
    public string Description => "Standard native graph preset set.";
    public IGraphPreset EncryptPreset { get; } = graphPresets.Single(x => x.Id == "ED387AE6-7CD4-4D86-9F91-DDB101E55DDD");
    public IGraphPreset DecryptPreset { get; } = graphPresets.Single(x => x.Id == "9B4C9B4A-E2FF-423E-9940-4699EA440734");
}
