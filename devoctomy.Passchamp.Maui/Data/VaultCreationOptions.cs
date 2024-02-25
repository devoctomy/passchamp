using System.Security;

namespace devoctomy.Passchamp.Maui.Data;

public class VaultCreationOptions
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string GraphPresetSetId { get; set; }
    public string CloudProviderId { get; set; }
    public string CloudProviderPath { get; set; }
    public SecureString Passphrase { get; set; }
}
