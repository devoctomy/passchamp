namespace devoctomy.Passchamp.Maui.Models;

public class VaultIndex
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string CloudProviderId { get; set; }
    public string CloudProviderPath { get; set; }
    public bool HasBeenUnlockedAtLeastOnce { get; set; }
}
