using devoctomy.Passchamp.Maui.Models;
using System.Security;

namespace devoctomy.Passchamp.Maui.Data;

public class VaultLoaderServiceOptions
{
    public VaultIndex VaultIndex { get; set; }
    public SecureString MasterPassphrase { get; set; }
}
